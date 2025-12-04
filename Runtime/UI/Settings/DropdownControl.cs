using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HexTecGames.UI
{
    public abstract class DropdownControl : AdvancedBehaviour
    {
        [SerializeField] protected TMP_Dropdown dropdown = default;


        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();
            if (dropdown == null)
            {
                dropdown = GetComponentInChildren<TMP_Dropdown>();
            }

            if (dropdown != null)
            {
                // Clear existing persistent listeners
                dropdown.onValueChanged.RemoveAllListeners();

                // Add a persistent listener (works in edit mode)
                UnityEditor.Events.UnityEventTools.AddPersistentListener(dropdown.onValueChanged, OnDropdownChanged);
                UnityEditor.EditorUtility.SetDirty(dropdown);
            }
#endif
        }
        protected virtual void Awake()
        {
            PopulateDropdown();
        }

        protected abstract void PopulateDropdown();
        public abstract void OnDropdownChanged(int index);
    }
}