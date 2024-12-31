using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.HotkeySystem
{
    public class HotkeyController : MonoBehaviour
    {
        private Dictionary<KeyCode, Action> hotkeys = new Dictionary<KeyCode, Action>();

        private static HotkeyController lastActiveController;

        private void OnEnable()
        {
            lastActiveController = this;
        }
        private void OnDisable()
        {
            if (lastActiveController == this)
            {
                lastActiveController = null;
            }
        }
        private void Update()
        {
            if (lastActiveController != this && lastActiveController != null)
            {
                return;
            }
            foreach (var hotkey in hotkeys)
            {
                if (Input.GetKeyDown(hotkey.Key))
                {
                    hotkey.Value.Invoke();
                }
            }
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