using System;
using System.Collections.Generic;
using Slots.Core;
using UnityEngine;

namespace Slots.Game
{
    // One play session: holds the credits and the bet, and charges and pays around each spin
    public class SlotGame : MonoBehaviour
    {
        public event EventHandler OnCreditsChanged;
        public event EventHandler OnBetChanged;
        public event EventHandler OnSpinStarted;
        public event EventHandler<OnSpinResolvedEventArgs> OnSpinResolved;
        public class OnSpinResolvedEventArgs : EventArgs
        {
            public SpinResult result;
            public int payout;
        }

        [SerializeField] private SlotConfigSO config;
        [SerializeField] private SlotMachine machine;
        [SerializeField] private GameInput gameInput;

        private Wallet wallet;
        private IReadOnlyList<int> bets;
        private int betIndex;

        public int Credits => wallet.Balance;
        public int CurrentBet => bets[betIndex];
        public bool IsSpinning => machine.IsSpinning;
        public bool CanSpin => !machine.IsSpinning && wallet.CanAfford(CurrentBet);
        public bool CanLowerBet => !machine.IsSpinning && betIndex > 0;
        public bool CanRaiseBet => !machine.IsSpinning && betIndex < bets.Count - 1 && wallet.CanAfford(bets[betIndex + 1]);

        private void Awake()
        {
            bets = config.Bets;
            wallet = new Wallet(config.StartingCredits);

            wallet.OnBalanceChanged += Wallet_OnBalanceChanged;
            machine.OnSpinCompleted += SlotMachine_OnSpinCompleted;
            gameInput.OnSpinPressed += GameInput_OnSpinPressed;
        }

        public void RequestSpin()
        {
            if (!CanSpin) return;

            wallet.TryDebit(CurrentBet);
            machine.Spin();

            OnSpinStarted?.Invoke(this, EventArgs.Empty);
        }

        public void LowerBet()
        {
            if (!CanLowerBet) return;

            betIndex--;
            OnBetChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseBet()
        {
            if (!CanRaiseBet) return;

            betIndex++;
            OnBetChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SlotMachine_OnSpinCompleted(object sender, SlotMachine.OnSpinCompletedEventArgs e)
        {
            int payout = e.result.PayoutFor(CurrentBet);
            if (payout > 0) wallet.Credit(payout);

            DropBetToAffordable();

            OnSpinResolved?.Invoke(this, new OnSpinResolvedEventArgs
            {
                result = e.result,
                payout = payout
            });
        }

        // A bet the player can no longer cover would leave spin disabled with no explanation
        private void DropBetToAffordable()
        {
            int startingIndex = betIndex;

            while (betIndex > 0 && !wallet.CanAfford(bets[betIndex]))
            {
                betIndex--;
            }

            if (betIndex != startingIndex) OnBetChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Wallet_OnBalanceChanged(object sender, EventArgs e)
        {
            OnCreditsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void GameInput_OnSpinPressed(object sender, EventArgs e)
        {
            RequestSpin();
        }

        private void OnDestroy()
        {
            wallet.OnBalanceChanged -= Wallet_OnBalanceChanged;
            machine.OnSpinCompleted -= SlotMachine_OnSpinCompleted;
            gameInput.OnSpinPressed -= GameInput_OnSpinPressed;
        }
    }
}
