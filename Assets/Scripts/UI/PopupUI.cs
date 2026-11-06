using PrimeTween;
using UnityEngine;

namespace Slots.Game
{
    // Shared behaviour for popups: unfolds them open from a flat line, folds them shut again,
    // and stops the game taking spin input behind them. Each popup only decides when to open
    // and what to show
    public abstract class PopupUI : MonoBehaviour
    {
        // How thin the panel is while it's still a line
        private const float FoldedHeight = 0.05f;

        [SerializeField] protected SlotGame game;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform panel;

        [Header("Unfold")]
        [SerializeField] private float widenDuration = 0.15f;
        [SerializeField] private float openDuration = 0.35f;

        [Header("Fold")]
        [SerializeField] private float closeDuration = 0.2f;
        [SerializeField] private float narrowDuration = 0.12f;

        [Header("Background Dim")]
        [SerializeField] private float dimDuration = 0.1f;

        private Sequence animation;

        protected bool IsOpen { get; private set; }

        protected virtual void Start()
        {
            canvasGroup.alpha = 0f;
            SetInteractive(false);
        }

        protected void Show()
        {
            IsOpen = true;
            game.SetInputBlocked(true);
            SetInteractive(true);

            StopAnimation();
            canvasGroup.alpha = 0f;
            panel.localScale = new Vector3(0f, FoldedHeight, 1f);

            animation = Sequence.Create()
                .Group(Tween.Alpha(canvasGroup, endValue: 1f, duration: dimDuration))
                .Group(Tween.ScaleX(panel, endValue: 1f, duration: widenDuration, ease: Ease.OutExpo))
                .Chain(Tween.ScaleY(panel, endValue: 1f, duration: openDuration, ease: Ease.OutBack));
        }

        protected void Hide()
        {
            if (!IsOpen) return;

            // Input comes back straight away, so closing never holds up the next spin
            IsOpen = false;
            game.SetInputBlocked(false);
            SetInteractive(false);

            StopAnimation();

            animation = Sequence.Create()
                .Chain(Tween.ScaleY(panel, endValue: FoldedHeight, duration: closeDuration, ease: Ease.InBack))
                .Chain(Tween.ScaleX(panel, endValue: 0f, duration: narrowDuration, ease: Ease.InExpo))
                .Chain(Tween.Alpha(canvasGroup, endValue: 0f, duration: dimDuration));
        }

        private void SetInteractive(bool isInteractive)
        {
            canvasGroup.interactable = isInteractive;
            canvasGroup.blocksRaycasts = isInteractive;
        }

        private void StopAnimation()
        {
            if (animation.isAlive) animation.Stop();
        }
    }
}
