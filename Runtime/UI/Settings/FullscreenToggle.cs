using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HexTecGames.UI
{
    public class FullscreenToggle : AdvancedBehaviour
    {
        [SerializeField] private Toggle toggle = default;

        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();
            if (toggle == null)
            {
                toggle = GetComponentInChildren<Toggle>();
            }

            if (toggle != null)
            {
                // Clear existing persistent listeners
                toggle.onValueChanged.RemoveAllListeners();

                // Add a persistent listener (works in edit mode)
                UnityEditor.Events.UnityEventTools.AddPersistentListener(toggle.onValueChanged, ToggleChanged);
                UnityEditor.EditorUtility.SetDirty(toggle);
            }
#endif
        }

        private void Awake()
        {
            toggle.SetIsOnWithoutNotify(Screen.fullScreen);
        }

        private void SetFullscreen(bool active)
        {
            Screen.fullScreen = active;
        }

        public void ToggleChanged(bool isOn)
        {
            SetFullscreen(isOn);
        }
    }
}