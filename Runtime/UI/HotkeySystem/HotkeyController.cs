using System;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.HotkeySystem
{
    public class HotkeyController : AdvancedBehaviour
    {
        private Dictionary<KeyCode, Action> hotkeys = new Dictionary<KeyCode, Action>();
        private int numberHotkeyCount;

        private void Start()
        {
            HotkeyUser[] results = FindObjectsOfType<HotkeyUser>(true);

            foreach (HotkeyUser result in results)
            {
                AddHotkey(result);
            }
        }

        private void Update()
        {
            foreach (KeyValuePair<KeyCode, Action> hotkey in hotkeys)
            {
                if (Input.GetKeyDown(hotkey.Key))
                {
                    hotkey.Value.Invoke();
                }
            }
        }

        public void AddNextNumberHotkey(Action action)
        {
            if (numberHotkeyCount >= SimpleHotkeys.alphaKeyCodes.Length)
            {
                return;
            }
            AddHotkey(SimpleHotkeys.alphaKeyCodes[numberHotkeyCount], action);
            numberHotkeyCount++;
        }

        public void AddHotkey(HotkeyUser btn)
        {
            AddHotkey(btn.KeyCode, btn.OnHotkeyPressed);
        }
        public void AddHotkey(KeyCode keyCode, Action action)
        {
            if (!hotkeys.TryAdd(keyCode, action))
            {
                hotkeys.TryGetValue(keyCode, out Action other);
                Debug.Log($"Hotkey already in use! Key: {keyCode}, first action: {other.Method}, second action: {action.Method}");
                return;
            }
        }
    }
}