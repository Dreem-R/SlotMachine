using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slots.Game
{
    // Offers a fresh start when the player can't afford even the lowest bet
    public class OutOfCreditsPopupUI : PopupUI
    {
        [SerializeField] private SlotConfigSO config;
        [SerializeField] private TMP_Text bodyText;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        protected override void Start()
        {
            base.Start();

            bodyText.text = $"Start again with {config.StartingCredits} credits?";

            yesButton.onClick.AddListener(RestartGame);
            noButton.onClick.AddListener(Hide);
            game.OnOutOfCredits += SlotGame_OnOutOfCredits;
        }

        private void SlotGame_OnOutOfCredits(object sender, EventArgs e)
        {
            Show();
        }

        private void RestartGame()
        {
            Hide();
            game.RestartSession();
        }

        private void OnDestroy()
        {
            game.OnOutOfCredits -= SlotGame_OnOutOfCredits;
        }
    }
}
