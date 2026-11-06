using System.Linq;
using Slots.Core;
using TMPro;
using UnityEngine;

namespace Slots.Game
{
    // Builds the paytable from the config, so it can't disagree with what the game really pays
    public class PaytableUI : MonoBehaviour
    {
        [SerializeField] private SlotConfigSO config;
        [SerializeField] private PaytableRowUI rowPrefab;
        [SerializeField] private Transform rowsParent;
        [SerializeField] private TMP_Text oddsText;

        private void Start()
        {
            foreach (SymbolSO symbol in config.Symbols.OrderByDescending(symbol => symbol.PayoutMultiplier))
            {
                Instantiate(rowPrefab, rowsParent).Setup(symbol);
            }

            (double rtp, double hitFrequency) = RtpCalculator.Calculate(config.BuildModel());
            oddsText.text = $"RTP {rtp * 100:F1}%  |  Win 1 in {1 / hitFrequency:F1} spins";
        }
    }
}
