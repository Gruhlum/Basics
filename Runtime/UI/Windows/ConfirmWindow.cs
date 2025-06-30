using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class ConfirmWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text textGUI = default;
        [Space]
        [SerializeField] private TMP_Text confirmGUI = default;
        [SerializeField] private TMP_Text cancelGUI = default;
        [Space]
        [SerializeField] private Button confirmButton = default;
        [SerializeField] private Button cancelButton = default;
        [Space]
        public UnityEvent OnConfirmClicked;
        public UnityEvent OnCancelClicked;

        [Space]
        [SerializeField, HideInInspector] private Button oldConfirmButton = default;
        [SerializeField, HideInInspector] private Button oldCancelButton = default;

        private Action confirmAction;
        private Action cancelAction;

#if UNITY_EDITOR
        private void OnValidate()
        {
            HandleEventListener(ref oldConfirmButton, confirmButton, ConfirmClicked);
            HandleEventListener(ref oldCancelButton, cancelButton, CancelClicked);
        }

        private void HandleEventListener(ref Button old, Button current, UnityAction method)
        {
            if (old != null)
            {
                UnityEditor.Events.UnityEventTools.RemovePersistentListener(old.onClick, method);
            }
            if (current != null)
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(current.onClick, method);
            }
            old = current;
        }
#endif
        public void Setup(string text, Action confirmAction, Action cancelAction, string confirmText = "Confirm", string cancelText = "Cancel")
        {
            Setup(text, confirmText, cancelText);
            this.confirmAction = confirmAction;
            this.cancelAction = cancelAction;
        }
        public void Setup(string text, string confirmText = "Confirm", string cancelText = "Cancel")
        {
            textGUI.text = text;
            confirmGUI.text = confirmText;
            cancelGUI.text = cancelText;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void ConfirmClicked()
        {
            if (confirmAction != null)
            {
                confirmAction.Invoke();
                confirmAction = null;
                cancelAction = null;
            }
            OnConfirmClicked?.Invoke();
            Deactivate();
        }
        private void CancelClicked()
        {
            if (cancelAction != null)
            {
                cancelAction.Invoke();
                cancelAction = null;
                confirmAction = null;
            }
            OnCancelClicked?.Invoke();
            Deactivate();
        }
    }
}