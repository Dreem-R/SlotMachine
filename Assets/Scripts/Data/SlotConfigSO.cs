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

        [Header("Session")]
        [SerializeField, Min(0)] private int startingCredits = 1000;
        [Tooltip("Lowest to highest")]
        [SerializeField] private int[] bets = { 10, 50, 100 };

        public IReadOnlyList<SymbolSO> Symbols => symbols;
        public int StartingCredits => startingCredits;
        public IReadOnlyList<int> Bets => bets;

        public SlotModel BuildModel()
        {
            // The core only knows a symbol by its position in the symbols list
            IEnumerable<ReelStrip> strips = reels.Select(reel => new ReelStrip(reel.stops.Select(stop => Array.IndexOf(symbols, stop))));
            Paytable paytable = new Paytable(symbols.Select(symbol => symbol.PayoutMultiplier));

            return new SlotModel(strips, paytable);
        }
    }
}
