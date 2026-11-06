using System;
using System.Collections.Generic;
using Slots.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Slots.Game
{
    // Draws one reel and animates it to the stop it's given, it never decides the result
    public class Reel : MonoBehaviour
    {
        public event EventHandler OnStopped;

        [SerializeField] private Image[] cells;
        [SerializeField] private float symbolSpacing = 104f;

        [Header("Spin Feel")]
        [SerializeField] private float targetSpeed = 14f;
        [SerializeField] private float accelerationDuration = 0.25f;
        [SerializeField] private float brakingDuration = 0.7f;
        [SerializeField] private float overshoot = 1f;

        private ReelStrip strip;
        private IReadOnlyList<SymbolSO> symbols;
        private ReelMotion motion;
        private float elapsed;
        private int targetStop;

        public int CurrentStop { get; private set; }

        public void Initialize(ReelStrip strip, IReadOnlyList<SymbolSO> symbols, int startStop)
        {
            this.strip = strip;
            this.symbols = symbols;

            CurrentStop = startStop;
            Render(startStop);
        }

        public void Spin(int targetStop, float duration)
        {
            this.targetStop = targetStop;

            motion = new ReelMotion(duration, targetSpeed, accelerationDuration, brakingDuration, overshoot);
            elapsed = 0f;
        }

        private void Update()
        {
            if (motion == null) return;

            elapsed += Time.deltaTime;

            if (elapsed >= motion.Duration)
            {
                FinishSpin();
                return;
            }

            // Once up to speed the strip jumps so the reel will arrive on the target exactly.
            // At this speed one symbol swap can't be seen, and it lets every spin take the same
            // time without changing the reel's speed
            int originStop = elapsed < motion.CruiseStartTime ? CurrentStop : targetStop - motion.Distance;

            Render(originStop + motion.Evaluate(elapsed));
        }

        private void FinishSpin()
        {
            motion = null;
            CurrentStop = targetStop;
            Render(targetStop);

            OnStopped?.Invoke(this, EventArgs.Empty);
        }

        private void Render(float position)
        {
            int baseStop = Mathf.FloorToInt(position);
            float fraction = position - baseStop;
            int middleCell = cells.Length / 2;

            for (int i = 0; i < cells.Length; i++)
            {
                int offset = i - middleCell;

                cells[i].sprite = symbols[strip.SymbolAt(baseStop + offset)].Sprite;
                cells[i].rectTransform.anchoredPosition = new Vector2(0f, (offset - fraction) * symbolSpacing);
            }
        }
    }
}
