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
        private const string OutOfCreditsMessage = "OUT OF CREDITS";

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

        [Header("Feel")]
        [SerializeField] private float creditsCountUpDuration = 0.6f;
        [SerializeField] private float creditsCountDownDuration = 0.25f;

        private int displayedCredits;

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
            game.OnSessionRestarted += SlotGame_OnSessionRestarted;

            messageText.text = ReadyMessage;
            ShowCredits(game.Credits);
            RefreshBet();
            RefreshButtons();
        }

        private void SlotGame_OnCreditsChanged(object sender, EventArgs e)
        {
            Tween.StopAll(onTarget: this);

            if (game.Credits == displayedCredits) return;

            // Winnings count up slowly so the payout feels earned, a bet comes off quickly
            float duration = game.Credits > displayedCredits ? creditsCountUpDuration : creditsCountDownDuration;

            Tween.Custom(this, startValue: displayedCredits, endValue: game.Credits, duration: duration,
                onValueChange: (hud, value) => hud.ShowCredits(Mathf.RoundToInt(value)));
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
                messageText.text = game.IsOutOfCredits ? OutOfCreditsMessage : NoWinMessage;
            }

            RefreshButtons();
        }

        private void SlotGame_OnSessionRestarted(object sender, EventArgs e)
        {
            messageText.text = ReadyMessage;
            RefreshButtons();
        }

        private void ShowCredits(int credits)
        {
            displayedCredits = credits;
            creditsText.text = credits.ToString();
        }

        private void RefreshBet()
        {
            betText.text = game.CurrentBet.ToString();
        }

        private void RefreshButtons()
        {
            // Stays pressable when out of credits, since that's how the restart offer comes back
            bool canPressSpin = game.CanSpin || game.IsOutOfCredits;

            spinButton.interactable = canPressSpin;
            leverButton.interactable = canPressSpin;
            lowerBetButton.interactable = game.CanLowerBet;
            raiseBetButton.interactable = game.CanRaiseBet;
        }

        private void OnDestroy()
        {
            game.OnCreditsChanged -= SlotGame_OnCreditsChanged;
            game.OnBetChanged -= SlotGame_OnBetChanged;
            game.OnSpinStarted -= SlotGame_OnSpinStarted;
            game.OnSpinResolved -= SlotGame_OnSpinResolved;
            game.OnSessionRestarted -= SlotGame_OnSessionRestarted;
        }
    }
}
