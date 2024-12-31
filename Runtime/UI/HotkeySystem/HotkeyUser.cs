using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HexTecGames.HotkeySystem
{
    public abstract class HotkeyUser : MonoBehaviour
    {
        [SerializeField] private HotkeyController hotkeyController = default;
        [SerializeField] private KeyCode keyCode = default;

        protected virtual void Start()
        {
            SetupHotkey();
        }

        private void SetupHotkey()
        {
            hotkeyController.AddHotkey(keyCode, OnHotkeyPressed);
        }

        protected abstract void OnHotkeyPressed();
    }
}