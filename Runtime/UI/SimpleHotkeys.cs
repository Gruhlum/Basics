using UnityEngine;

namespace HexTecGames
{
    public static class SimpleHotkeys
    {
        public static KeyCode[] alphaKeyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
        public static KeyCode[] keypadKeyCodes = new KeyCode[] { KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0 };

        public static KeyCode[] qwertyKeyCodes = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };
        public static KeyCode[] PoEKeyCodes = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Q, KeyCode.E, KeyCode.R, KeyCode.F, KeyCode.X,
        KeyCode.C, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 ,KeyCode.Alpha5};

        public static KeyCode GetPoEHotkey(int index)
        {
            return GetHotkey(PoEKeyCodes, index);
        }
        public static KeyCode GeAlphaHotkey(int index)
        {
            return GetHotkey(alphaKeyCodes, index);
        }
        public static KeyCode GeKeypadHotkey(int index)
        {
            return GetHotkey(keypadKeyCodes, index);
        }
        public static KeyCode GetQwertyHotkey(int index)
        {
            return GetHotkey(qwertyKeyCodes, index);
        }

        private static KeyCode GetHotkey(KeyCode[] array, int index)
        {
            if (index >= array.Length)
            {
                return KeyCode.None;
            }
            return array[index];
        }

        public static bool HotkeyDown(int number)
        {
            if (!IsValidNumberInput(number, 10))
            {
                return false;
            }
            return Input.GetKeyDown(alphaKeyCodes[number]) || Input.GetKeyDown(keypadKeyCodes[number]);
        }
        public static bool HotkeyHeld(int number)
        {
            if (!IsValidNumberInput(number, 10))
            {
                return false;
            }
            return Input.GetKey(alphaKeyCodes[number]) || Input.GetKey(keypadKeyCodes[number]);
        }
        public static bool HotkeyUp(int number)
        {
            if (!IsValidNumberInput(number, 10))
            {
                return false;
            }
            return Input.GetKeyUp(alphaKeyCodes[number]) || Input.GetKeyUp(keypadKeyCodes[number]);
        }

        private static bool IsValidNumberInput(int number, int max)
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