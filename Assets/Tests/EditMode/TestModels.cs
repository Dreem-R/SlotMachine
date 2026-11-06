using System.Collections.Generic;
using System.Linq;

namespace Slots.Core.Tests
{
    // The paytable from the design, built by hand so tests don't rely on project assets
    internal static class TestModels
    {
        public const int Cherry = 0;
        public const int Bell = 1;
        public const int Bar = 2;
        public const int Seven = 3;

        public static SlotModel CreateStandardMachine()
        {
            List<int> stops = new List<int>();
            stops.AddRange(Enumerable.Repeat(Cherry, 9));
            stops.AddRange(Enumerable.Repeat(Bell, 6));
            stops.AddRange(Enumerable.Repeat(Bar, 3));
            stops.AddRange(Enumerable.Repeat(Seven, 2));

            ReelStrip[] reels = { new ReelStrip(stops), new ReelStrip(stops), new ReelStrip(stops) };
            return new SlotModel(reels, new Paytable(new[] { 4, 10, 30, 200 }));
        }
    }
}
