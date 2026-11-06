using System;
using NUnit.Framework;

namespace Slots.Core.Tests
{
    public class RtpTests
    {
        // Seeded so a failure can be reproduced exactly
        private sealed class SeededRandomSource : IRandomSource
        {
            private readonly Random random;

            public SeededRandomSource(int seed)
            {
                random = new Random(seed);
            }

            public int Next(int maxExclusive)
            {
                return random.Next(maxExclusive);
            }
        }

        [Test]
        public void Calculate_ChecksEveryCombination()
        {
            RtpReport report = RtpCalculator.Calculate(TestModels.CreateStandardMachine());

            Assert.That(report.Combinations, Is.EqualTo(20L * 20L * 20L));
        }

        [Test]
        public void Calculate_MatchesTheHandWorkedPaytable()
        {
            // Each symbol: (stops / 20)^3 * multiplier, worked out separately from the game code
            double expectedRtp =
                Math.Pow(9 / 20.0, 3) * 4 +
                Math.Pow(6 / 20.0, 3) * 10 +
                Math.Pow(3 / 20.0, 3) * 30 +
                Math.Pow(2 / 20.0, 3) * 200;

            RtpReport report = RtpCalculator.Calculate(TestModels.CreateStandardMachine());

            Assert.That(expectedRtp, Is.EqualTo(0.93575).Within(1e-12));
            Assert.That(report.Rtp, Is.EqualTo(expectedRtp).Within(1e-12));
            Assert.That(report.HitFrequency, Is.EqualTo(0.1225).Within(1e-12));
        }

        [Test]
        public void Simulation_OfRealSpins_AgreesWithTheExactRtp()
        {
            const int Spins = 1000000;

            // A single spin's payout has a standard deviation of about 6.81x the bet,
            // so three standard errors over a million spins is about 2 percentage points
            const double RtpTolerance = 0.0205;
            const double HitTolerance = 0.001;

            SlotEngine engine = new SlotEngine(TestModels.CreateStandardMachine(), new SeededRandomSource(20261106));
            long totalMultiplier = 0;
            int wins = 0;

            for (int i = 0; i < Spins; i++)
            {
                SpinResult result = engine.Spin();
                totalMultiplier += result.PayoutMultiplier;
                if (result.IsWin) wins++;
            }

            Assert.That((double)totalMultiplier / Spins, Is.EqualTo(0.93575).Within(RtpTolerance));
            Assert.That((double)wins / Spins, Is.EqualTo(0.1225).Within(HitTolerance));
        }
    }
}
