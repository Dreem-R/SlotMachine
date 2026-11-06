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
            if (startingBalance < 0) throw new ArgumentOutOfRangeException(nameof(startingBalance));

            Balance = startingBalance;
        }

        public bool CanAfford(int amount)
        {
            return amount <= Balance;
        }

        public bool TryDebit(int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (!CanAfford(amount)) return false;

            Balance -= amount;
            OnBalanceChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public void Credit(int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            Balance += amount;
            OnBalanceChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
