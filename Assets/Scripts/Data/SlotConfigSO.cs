using System;
using System.Collections.Generic;
using System.Linq;
using Slots.Core;
using UnityEngine;

namespace Slots.Game
{
    // The machine's setup as an asset, so symbols, odds and payouts change without touching code
    [CreateAssetMenu(menuName = "Slots/Slot Config")]
    public class SlotConfigSO : ScriptableObject
    {
        [Serializable]
        private class ReelStripDefinition
        {
            public SymbolSO[] stops;
        }

        [SerializeField] private SymbolSO[] symbols;
        [SerializeField] private ReelStripDefinition[] reels;

        public IReadOnlyList<SymbolSO> Symbols => symbols;

        public SlotModel BuildModel()
        {
            // The core only knows a symbol by its position in the symbols list
            IEnumerable<ReelStrip> strips = reels.Select(reel => new ReelStrip(reel.stops.Select(stop => Array.IndexOf(symbols, stop))));
            Paytable paytable = new Paytable(symbols.Select(symbol => symbol.PayoutMultiplier));

            return new SlotModel(strips, paytable);
        }
    }
}
