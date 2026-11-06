using System;
using Slots.Core;
using UnityEngine;

namespace Slots.Game
{
    // Asks the engine for a result, then tells the reels where to land
    public class SlotMachine : MonoBehaviour
    {
        public event EventHandler OnSpinStarted;
        public event EventHandler<OnSpinCompletedEventArgs> OnSpinCompleted;
        public class OnSpinCompletedEventArgs : EventArgs
        {
            public SpinResult result;
        }

        [SerializeField] private SlotConfigSO config;
        [SerializeField] private Reel[] reels;

        [Header("Spin Timing")]
        [SerializeField] private float firstReelDuration = 1.3f;
        [SerializeField] private float reelStopStagger = 0.4f;

        private SlotEngine engine;
        private SpinResult currentResult;
        private int reelsStillSpinning;

        public bool IsSpinning => reelsStillSpinning > 0;

        private void Awake()
        {
            SlotModel model = config.BuildModel();
            engine = new SlotEngine(model, new SecureRandomSource());

            // The machine opens on random symbols rather than always looking the same
            SpinResult openingResult = engine.Spin();

            for (int i = 0; i < reels.Length; i++)
            {
                reels[i].Initialize(model.GetReel(i), config.Symbols, openingResult.Stops[i]);
                reels[i].OnStopped += Reel_OnStopped;
            }
        }

        public void Spin()
        {
            if (IsSpinning) return;

            // The result is final from here on, the animation only displays it
            currentResult = engine.Spin();
            reelsStillSpinning = reels.Length;

            for (int i = 0; i < reels.Length; i++)
            {
                reels[i].Spin(currentResult.Stops[i], firstReelDuration + i * reelStopStagger);
            }

            OnSpinStarted?.Invoke(this, EventArgs.Empty);
        }

        private void Reel_OnStopped(object sender, EventArgs e)
        {
            reelsStillSpinning--;

            if (reelsStillSpinning == 0)
            {
                OnSpinCompleted?.Invoke(this, new OnSpinCompletedEventArgs
                {
                    result = currentResult
                });
            }
        }

        private void OnDestroy()
        {
            foreach (Reel reel in reels)
            {
                reel.OnStopped -= Reel_OnStopped;
            }
        }
    }
}
