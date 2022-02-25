using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static Controls InputActions = new Controls();
        public static event Action<InputActionMap> ActionMapChange;

        private void Start() => ToggleActionMap(InputActions.Player);

        public static void ToggleActionMap(InputActionMap actionMap)
        {
            if (actionMap.enabled) return;
            InputActions.Disable();
            ActionMapChange?.Invoke(actionMap);
            actionMap.Enable();
        }
    }
}