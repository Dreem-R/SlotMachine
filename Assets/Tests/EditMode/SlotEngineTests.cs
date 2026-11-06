using System.Collections.Generic;
using NUnit.Framework;

namespace Slots.Core.Tests
{
    public class SlotEngineTests
    {
        // Returns planned values and records each limit it was asked for
        private sealed class ScriptedRandomSource : IRandomSource
        {
            private readonly Queue<int> values;

            public List<int> RequestedLimits { get; } = new List<int>();

            public ScriptedRandomSource(params int[] values)
            {
                this.values = new Queue<int>(values);
            }

            public int Next(int maxExclusive)
            {
                RequestedLimits.Add(maxExclusive);
                return values.Dequeue();
            }
        }

        [Test]
        public void Spin_DrawsOneStopPerReel_FromThatReelsLength()
        {
            ReelStrip[] reels =
            {
                new ReelStrip(new[] { 0, 1 }),
                new ReelStrip(new[] { 0, 1, 1 }),
                new ReelStrip(new[] { 0, 1, 1, 1 })
            };
            SlotModel model = new SlotModel(reels, new Paytable(new[] { 2, 3 }));
            ScriptedRandomSource random = new ScriptedRandomSource(1, 2, 3);

            SpinResult result = new SlotEngine(model, random).Spin();

            Assert.That(random.RequestedLimits, Is.EqualTo(new[] { 2, 3, 4 }));
            Assert.That(result.Stops, Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(result.WinningSymbol, Is.EqualTo(1));
        }

        [Test]
        public void SecureRandomSource_StaysInRange_AndReachesEveryValue()
        {
            SecureRandomSource random = new SecureRandomSource();
            bool[] seen = new bool[5];

            for (int i = 0; i < 1000; i++)
            {
                int value = random.Next(seen.Length);
                Assert.That(value, Is.InRange(0, seen.Length - 1));
                seen[value] = true;
            }

            Assert.That(seen, Is.All.True);
        }
    }
}
