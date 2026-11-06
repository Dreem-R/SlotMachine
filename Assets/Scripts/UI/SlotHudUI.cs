using System;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slots.Game
{
    // Shows credits, the bet and the last result, and sends button presses to the game
    public class SlotHudUI : MonoBehaviour
    {
        private const string ReadyMessage = "PRESS SPIN";
        private const string SpinningMessage = "GOOD LUCK!";
        private const string NoWinMessage = "NO WIN";

        [SerializeField] private SlotGame game;

        [Header("Displays")]
        [SerializeField] private TMP_Text creditsText;
        [SerializeField] private TMP_Text betText;
        [SerializeField] private TMP_Text messageText;

        [Header("Buttons")]
        [SerializeField] private Button spinButton;
        [SerializeField] private Button leverButton;
        [SerializeField] private Button lowerBetButton;
        [SerializeField] private Button raiseBetButton;

        private void Start()
        {
            spinButton.onClick.AddListener(game.RequestSpin);
            leverButton.onClick.AddListener(game.RequestSpin);
            lowerBetButton.onClick.AddListener(game.LowerBet);
            raiseBetButton.onClick.AddListener(game.RaiseBet);

            game.OnCreditsChanged += SlotGame_OnCreditsChanged;
            game.OnBetChanged += SlotGame_OnBetChanged;
            game.OnSpinStarted += SlotGame_OnSpinStarted;
            game.OnSpinResolved += SlotGame_OnSpinResolved;

            messageText.text = ReadyMessage;
            RefreshCredits();
            RefreshBet();
            RefreshButtons();
        }

        private void SlotGame_OnCreditsChanged(object sender, EventArgs e)
        {
            RefreshCredits();
        }

        private void SlotGame_OnBetChanged(object sender, EventArgs e)
        {
            RefreshBet();
            RefreshButtons();
        }

        private void SlotGame_OnSpinStarted(object sender, EventArgs e)
        {
            messageText.text = SpinningMessage;
            RefreshButtons();
        }

        private void SlotGame_OnSpinResolved(object sender, SlotGame.OnSpinResolvedEventArgs e)
        {
            if (e.payout > 0)
            {
                messageText.text = $"YOU WIN {e.payout}!";
                Tween.PunchScale(messageText.transform, strength: Vector3.one * 0.25f, duration: 0.4f);
            }
            else
            {
                messageText.text = NoWinMessage;
            }

            RefreshButtons();
        }

        private void RefreshCredits()
        {
            creditsText.text = game.Credits.ToString();
        }

        private void RefreshBet()
        {
            betText.text = game.CurrentBet.ToString();
        }

        private void RefreshButtons()
        {
            spinButton.interactable = game.CanSpin;
            leverButton.interactable = game.CanSpin;
            lowerBetButton.interactable = game.CanLowerBet;
            raiseBetButton.interactable = game.CanRaiseBet;
        }

        private void OnDestroy()
        {
            game.OnCreditsChanged -= SlotGame_OnCreditsChanged;
            game.OnBetChanged -= SlotGame_OnBetChanged;
            game.OnSpinStarted -= SlotGame_OnSpinStarted;
            game.OnSpinResolved -= SlotGame_OnSpinResolved;
        }
    }
}
