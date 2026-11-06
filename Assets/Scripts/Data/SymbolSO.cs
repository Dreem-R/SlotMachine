using UnityEngine;

namespace Slots.Game
{
    // One slot symbol: how it looks and what three of them pay
    [CreateAssetMenu(menuName = "Slots/Symbol")]
    public class SymbolSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField, Min(0)] private int payoutMultiplier;

        public Sprite Sprite => sprite;
        public int PayoutMultiplier => payoutMultiplier;
    }
}
