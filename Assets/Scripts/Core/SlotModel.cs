using System;
using System.Collections.Generic;
using System.Linq;

namespace Slots.Core
{
    // The rules of the machine: live spins and the RTP calculation both use Resolve,
    // so the calculated odds can't drift away from how the game really pays
    public sealed class SlotModel
    {
        private readonly ReelStrip[] reels;
        private readonly Paytable paytable;

        public int ReelCount => reels.Length;

        public SlotModel(IEnumerable<ReelStrip> reels, Paytable paytable)
        {
            this.reels = reels.ToArray();
            this.paytable = paytable;

            // A symbol with no payout is a mistake in the config, so catch it at startup
            foreach (ReelStrip reel in this.reels)
            {
                for (int stop = 0; stop < reel.Length; stop++)
                {
                    int symbol = reel.SymbolAt(stop);
                    if (symbol < 0 || symbol >= paytable.SymbolCount)
                    {
                        throw new ArgumentException($"Symbol {symbol} isn't in the paytable", nameof(reels));
                    }
                }
            }
        }

        public ReelStrip GetReel(int reelIndex)
        {
            return reels[reelIndex];
        }

        public SpinResult Resolve(IReadOnlyList<int> stops)
        {
            int[] centerSymbols = new int[reels.Length];
            for (int i = 0; i < reels.Length; i++)
            {
                centerSymbols[i] = reels[i].SymbolAt(stops[i]);
            }

            int firstSymbol = centerSymbols[0];
            bool allMatch = centerSymbols.All(symbol => symbol == firstSymbol);

            return allMatch
                ? new SpinResult(stops, centerSymbols, firstSymbol, paytable.MultiplierFor(firstSymbol))
                : new SpinResult(stops, centerSymbols, SpinResult.NoSymbol, 0);
        }
    }
}
