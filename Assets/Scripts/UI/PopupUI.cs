using PrimeTween;
using UnityEngine;

namespace Slots.Game
{
    // Shared behaviour for popups: fades and scales them in and out, and stops the game
    // taking spin input behind them. Each popup only decides when to open and what to show
    public abstract class PopupUI : MonoBehaviour
    {
        [SerializeField] protected SlotGame game;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform panel;
        [SerializeField] private float showDuration = 0.3f;
        [SerializeField] private float hideDuration = 0.15f;

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

            StopTweens();
            Tween.Alpha(canvasGroup, endValue: 1f, duration: showDuration);
            Tween.Scale(panel, startValue: Vector3.one * 0.7f, endValue: Vector3.one, duration: showDuration, ease: Ease.OutBack);
        }

        protected void Hide()
        {
            if (!IsOpen) return;

            IsOpen = false;
            game.SetInputBlocked(false);
            SetInteractive(false);

            StopTweens();
            Tween.Alpha(canvasGroup, endValue: 0f, duration: hideDuration);
        }

        private void SetInteractive(bool isInteractive)
        {
            canvasGroup.interactable = isInteractive;
            canvasGroup.blocksRaycasts = isInteractive;
        }

        private void StopTweens()
        {
            Tween.StopAll(onTarget: canvasGroup);
            Tween.StopAll(onTarget: panel);
        }
    }
}
