using System.Collections.Generic;
using System.Linq;

namespace Slots.Core
{
    // The looping order of symbols on one reel, repeating a symbol is what makes it more likely
    public sealed class ReelStrip
    {
        private readonly int[] symbols;

        public int Length => symbols.Length;

        public ReelStrip(IEnumerable<int> symbols)
        {
            this.symbols = symbols.ToArray();
        }

        public int SymbolAt(int stop)
        {
            // Double modulo so the row above stop 0 wraps round to the end of the strip
            return symbols[(stop % Length + Length) % Length];
        }
    }
}
