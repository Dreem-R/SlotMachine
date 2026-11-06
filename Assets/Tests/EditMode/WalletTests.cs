using System;
using NUnit.Framework;

namespace Slots.Core.Tests
{
    public class WalletTests
    {
        [Test]
        public void TryDebit_WithEnoughCredits_TakesTheAmount()
        {
            Wallet wallet = new Wallet(100);

            Assert.That(wallet.TryDebit(40), Is.True);
            Assert.That(wallet.Balance, Is.EqualTo(60));
        }

        [Test]
        public void TryDebit_WithTooFewCredits_ChangesNothing()
        {
            Wallet wallet = new Wallet(30);
            int changes = 0;
            wallet.OnBalanceChanged += (sender, e) => changes++;

            Assert.That(wallet.TryDebit(50), Is.False);
            Assert.That(wallet.Balance, Is.EqualTo(30));
            Assert.That(changes, Is.Zero);
        }

        [Test]
        public void TryDebit_ExactBalance_LeavesZero()
        {
            Wallet wallet = new Wallet(50);

            Assert.That(wallet.TryDebit(50), Is.True);
            Assert.That(wallet.Balance, Is.Zero);
        }

        [Test]
        public void Credit_AddsTheAmount_AndRaisesTheEvent()
        {
            Wallet wallet = new Wallet(10);
            int changes = 0;
            wallet.OnBalanceChanged += (sender, e) => changes++;

            wallet.Credit(25);

            Assert.That(wallet.Balance, Is.EqualTo(35));
            Assert.That(changes, Is.EqualTo(1));
        }

        [Test]
        public void ZeroOrNegativeAmounts_Throw()
        {
            Wallet wallet = new Wallet(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => wallet.TryDebit(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => wallet.Credit(-5));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Wallet(-1));
        }
    }
}
