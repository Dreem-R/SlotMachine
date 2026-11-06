using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slots.Game
{
    // Celebrates the bigger wins, smaller ones only show in the message display so that
    // a popup doesn't interrupt play every few spins
    public class WinPopupUI : PopupUI
    {
        [SerializeField] private SlotConfigSO config;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private Image[] symbolIcons;
        [SerializeField] private TMP_Text payoutText;
        [SerializeField] private Button closeButton;

        protected override void Start()
        {
            base.Start();

            closeButton.onClick.AddListener(Hide);
            game.OnSpinResolved += SlotGame_OnSpinResolved;
            gameInput.OnSpinPressed += GameInput_OnSpinPressed;
        }

        private void SlotGame_OnSpinResolved(object sender, SlotGame.OnSpinResolvedEventArgs e)
        {
            if (!e.result.IsWin || e.result.PayoutMultiplier < config.BigWinMultiplier) return;

            foreach (Image icon in symbolIcons)
            {
                icon.sprite = config.Symbols[e.result.WinningSymbol].Sprite;
            }

            payoutText.text = $"+{e.payout} CREDITS";
            Show();
        }

        // SlotGame subscribes first, so it sees the input as still blocked and this
        // same key press closes the popup without also starting a spin
        private void GameInput_OnSpinPressed(object sender, EventArgs e)
        {
            if (IsOpen) Hide();
        }

        private void OnDestroy()
        {
            game.OnSpinResolved -= SlotGame_OnSpinResolved;
            gameInput.OnSpinPressed -= GameInput_OnSpinPressed;
        }
    }
}
