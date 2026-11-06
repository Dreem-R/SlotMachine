using System.Collections.Generic;

namespace Slots.Core
{
    // The decided outcome of one spin, the reels only animate towards it
    public sealed class SpinResult
    {
        public const int NoSymbol = -1;

        public IReadOnlyList<int> Stops { get; }
        public IReadOnlyList<int> CenterSymbols { get; }
        public int WinningSymbol { get; }
        public int PayoutMultiplier { get; }

        public bool IsWin => WinningSymbol != NoSymbol;

        public SpinResult(IReadOnlyList<int> stops, IReadOnlyList<int> centerSymbols, int winningSymbol, int payoutMultiplier)
        {
            Stops = stops;
            CenterSymbols = centerSymbols;
            WinningSymbol = winningSymbol;
            PayoutMultiplier = payoutMultiplier;
        }

        public int PayoutFor(int bet)
        {
            return bet * PayoutMultiplier;
        }
    }
}
