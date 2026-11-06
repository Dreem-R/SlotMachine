using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Slots.Game
{
    // Turns key presses into events, so nothing else needs to read the keyboard
    public class GameInput : MonoBehaviour
    {
        public event EventHandler OnSpinPressed;

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                OnSpinPressed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
