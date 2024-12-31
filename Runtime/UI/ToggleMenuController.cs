using HexTecGames.HotkeySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class ToggleMenuController : MonoBehaviour
    {
        [SerializeField] private HotkeyController hotkeyController = default;
        [SerializeField] private KeyCode keyCode = KeyCode.F1;
        [SerializeField] private GameObject canvas = default;
        [SerializeField] private bool loadState = default;

        [SerializeField, DrawIf(nameof(loadState), true)] private string saveString;

        public event Action<ToggleMenuController, bool> OnStateChanged;
        public event Action<ToggleMenuController> OnDestroyed;

        protected virtual void Reset()
        {
            saveString = this.name + "_IsActive";
        }

        protected virtual void Start()
        {
            if (hotkeyController != null)
            {
                hotkeyController.AddHotkey(keyCode, Toggle);
            }

            if (!loadState)
            {
                return;
            }

            bool isActive = canvas.activeSelf;

            if (SaveSystem.LoadSettings(saveString, ref isActive))
            {
                SetActive(isActive);
            }
        }
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Toggle();
            }
        }

        protected virtual void OnDestroy()
        {
            if (loadState)
            {
                SaveSystem.SaveSettings(saveString, canvas.activeSelf);
            }
            OnDestroyed?.Invoke(this);
        }

        public void Toggle()
        {
            SetActive(!canvas.activeSelf);
        }
        public void SetActive(bool active)
        {
            canvas.SetActive(active);
            OnStateChanged?.Invoke(this, active);
        }
    }
}