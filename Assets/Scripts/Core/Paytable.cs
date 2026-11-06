using System;
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
            if (multipliers == null) throw new ArgumentNullException(nameof(multipliers));

            this.multipliers = multipliers.ToArray();

            if (this.multipliers.Length == 0) throw new ArgumentException("A paytable needs at least one symbol", nameof(multipliers));
            if (this.multipliers.Any(multiplier => multiplier < 0)) throw new ArgumentException("Multipliers can't be negative", nameof(multipliers));
        }

        public int MultiplierFor(int symbol)
        {
            return multipliers[symbol];
        }
    }
}
