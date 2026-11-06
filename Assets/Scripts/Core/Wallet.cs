using System;

namespace Slots.Core
{
    // The player's credits, simulated only, no real money is involved
    public sealed class Wallet
    {
        public event EventHandler OnBalanceChanged;

        public int Balance { get; private set; }

        public Wallet(int startingBalance)
        {
            Balance = startingBalance;
        }

        public bool CanAfford(int amount)
        {
            return amount <= Balance;
        }

        public bool TryDebit(int amount)
        {
            if (!CanAfford(amount)) return false;

            Balance -= amount;
            OnBalanceChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public void Credit(int amount)
        {
            Balance += amount;
            OnBalanceChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
