using System;
using PrimeTween;
using UnityEngine;

namespace Slots.Game
{
    // Pulls the lever down for a moment whenever a spin starts
    public class LeverVisual : MonoBehaviour
    {
        [SerializeField] private SlotMachine machine;
        [SerializeField] private GameObject leverUp;
        [SerializeField] private GameObject leverDown;
        [SerializeField] private float pulledDuration = 0.25f;

        private void Awake()
        {
            machine.OnSpinStarted += SlotMachine_OnSpinStarted;
        }

        private void SlotMachine_OnSpinStarted(object sender, EventArgs e)
        {
            SetPulled(true);
            Tween.Delay(this, pulledDuration, lever => lever.SetPulled(false));
        }

        private void SetPulled(bool isPulled)
        {
            leverUp.SetActive(!isPulled);
            leverDown.SetActive(isPulled);
        }

        private void OnDestroy()
        {
            machine.OnSpinStarted -= SlotMachine_OnSpinStarted;
        }
    }
}
