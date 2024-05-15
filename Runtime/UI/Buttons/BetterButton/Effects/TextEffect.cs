using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    [System.Serializable]
    public class TextEffect : ButtonEffect
    {
        [SerializeField] private TMP_Text textGUI = default;
        [SerializeField] private string newText = default;

        private string oldText;

        public override void OnValidate(GameObject go)
        {
            base.OnValidate(go);
            if (textGUI == null)
            {
                textGUI = go.GetComponent<TMP_Text>();
            }
            if (textGUI == null)
            {
                textGUI = go.GetComponentInParent<TMP_Text>();
            }
        }
        public override void Apply()
        {
            oldText = textGUI.text;
            textGUI.text = newText;
        }

        public override void Remove()
        {
            if (string.IsNullOrEmpty(oldText))
            {
                return;
            }
            textGUI.text = oldText;
            oldText = null;
        }
    }
}