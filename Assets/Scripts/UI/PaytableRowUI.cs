using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slots.Game
{
    // One line of the paytable: three of a symbol and what they pay
    public class PaytableRowUI : MonoBehaviour
    {
        [SerializeField] private Image[] icons;
        [SerializeField] private TMP_Text payoutText;

        public void Setup(SymbolSO symbol)
        {
            foreach (Image icon in icons)
            {
                icon.sprite = symbol.Sprite;
            }

            payoutText.text = $"{symbol.PayoutMultiplier}x";
        }
    }
}
