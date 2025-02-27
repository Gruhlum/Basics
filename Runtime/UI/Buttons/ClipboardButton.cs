using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    public class ClipboardButton : BaseButton
    {
        [SerializeField] private TMP_Text textGUI = default;
        [SerializeField] private bool useTextGUI = default;
        [DrawIf(nameof(useTextGUI), false)] public string clipboardText;
        public bool showConfirmation = true;
        [DrawIf(nameof(showConfirmation), true)] public string confirmationText = "Copied!";

        private bool isAnimating;


        void OnDisable()
        {
            isAnimating = false;
        }

        protected override void Reset()
        {
            base.Reset();
            if (!TryGetComponent(out textGUI))
            {
                textGUI = GetComponentInChildren<TMP_Text>();
            }
            if (textGUI != null)
            {
                clipboardText = textGUI.text;
            }
        }

        protected override void ClickEffect()
        {
            if (useTextGUI)
            {
                GUIUtility.systemCopyBuffer = textGUI.text;
            }
            else GUIUtility.systemCopyBuffer = clipboardText;
            if (textGUI != null && showConfirmation)
            {
                StartCoroutine(AnimateConfirmText());
            }
        }
        private IEnumerator AnimateConfirmText()
        {
            if (isAnimating)
            {
                yield break;
            }
            isAnimating = true;
            string oldText = textGUI.text;
            textGUI.text = confirmationText;

            yield return new WaitForSeconds(0.3f);

            textGUI.text = oldText;
            isAnimating = false;
        }
    }
}