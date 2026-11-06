using System.Collections.Generic;
using System.Linq;

namespace Slots.Core
{
    // What each symbol pays, as a multiple of the bet, when every reel lands on it
    public sealed class Paytable
    {
        private readonly int[] multipliers;

        public int SymbolCount => multipliers.Length;

        public Paytable(IEnumerable<int> multipliers)
        {
            this.multipliers = multipliers.ToArray();
        }

        public int MultiplierFor(int symbol)
        {
            return multipliers[symbol];
        }
    }
}
