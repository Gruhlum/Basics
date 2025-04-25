using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    public static class SimpleHotkeys
    {
        public static KeyCode[] alphaKeycodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
        public static KeyCode[] keypadKeycodes = new KeyCode[] { KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0 };

        public static KeyCode[] qwerty = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };

        public static bool HotkeyDown(int number)
        {
            if (!isValidNumberInput(number, 10))
            {
                return false;
            }
            return Input.GetKeyDown(alphaKeycodes[number]) || Input.GetKeyDown(keypadKeycodes[number]);
        }
        public static bool HotkeyHeld(int number)
        {
            if (!isValidNumberInput(number, 10))
            {
                return false;
            }
            return Input.GetKey(alphaKeycodes[number]) || Input.GetKey(keypadKeycodes[number]);
        }
        public static bool HotkeyUp(int number)
        {
            if (!isValidNumberInput(number, 10))
            {
                return false;
            }
            return Input.GetKeyUp(alphaKeycodes[number]) || Input.GetKeyUp(keypadKeycodes[number]);
        }

        private static bool isValidNumberInput(int number, int max)
        {
            if (number < 0)
            {
                Debug.Log($"Number has to be between 0 and {max}");
                return false;
            }
            if (number >= max)
            {
                Debug.Log($"Number has to be between 0 and {max}");
                return false;
            }
            return true;
        }
    }
}