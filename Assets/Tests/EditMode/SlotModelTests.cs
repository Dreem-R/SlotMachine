using System;
using NUnit.Framework;

namespace Slots.Core.Tests
{
    public class SlotModelTests
    {
        private static SlotModel CreateModel()
        {
            ReelStrip[] reels =
            {
                new ReelStrip(new[] { 0, 1, 2 }),
                new ReelStrip(new[] { 0, 1, 2 }),
                new ReelStrip(new[] { 2, 1, 0 })
            };
            return new SlotModel(reels, new Paytable(new[] { 5, 20, 100 }));
        }

        [Test]
        public void Resolve_AllReelsMatch_PaysThatSymbolsMultiplier()
        {
            SpinResult result = CreateModel().Resolve(new[] { 1, 1, 1 });

            Assert.That(result.IsWin, Is.True);
            Assert.That(result.WinningSymbol, Is.EqualTo(1));
            Assert.That(result.PayoutMultiplier, Is.EqualTo(20));
            Assert.That(result.PayoutFor(50), Is.EqualTo(1000));
        }

        [Test]
        public void Resolve_TwoOfThreeMatch_IsNotAWin()
        {
            SpinResult result = CreateModel().Resolve(new[] { 0, 0, 0 });

            Assert.That(result.CenterSymbols, Is.EqualTo(new[] { 0, 0, 2 }));
            Assert.That(result.IsWin, Is.False);
            Assert.That(result.WinningSymbol, Is.EqualTo(SpinResult.NoSymbol));
            Assert.That(result.PayoutFor(50), Is.Zero);
        }

        [Test]
        public void Resolve_CallerChangesItsArrayAfterwards_ResultStaysTheSame()
        {
            int[] stops = { 1, 1, 1 };
            SpinResult result = CreateModel().Resolve(stops);

            stops[0] = 2;

            Assert.That(result.Stops, Is.EqualTo(new[] { 1, 1, 1 }));
        }

        [Test]
        public void Resolve_WrongNumberOfStops_Throws()
        {
            Assert.Throws<ArgumentException>(() => CreateModel().Resolve(new[] { 0, 0 }));
        }

        [Test]
        public void Constructor_SymbolMissingFromPaytable_Throws()
        {
            ReelStrip[] reels = { new ReelStrip(new[] { 0, 5 }) };

            Assert.Throws<ArgumentException>(() => new SlotModel(reels, new Paytable(new[] { 1 })));
        }

        [Test]
        public void ReelStrip_StopsOutsideTheStrip_WrapAround()
        {
            ReelStrip strip = new ReelStrip(new[] { 7, 8, 9 });

            Assert.That(strip.SymbolAt(3), Is.EqualTo(7));
            Assert.That(strip.SymbolAt(-1), Is.EqualTo(9));
        }
    }
}
