using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class DisplayableDisplay : Display<DisplayableObject>
    {
        [SerializeField] private TMP_Text nameGUI = default;
        [SerializeField] private Image icon = default;
        [SerializeField] private Image background = default;

        [SerializeField] private Color backgroundColor = Color.white;
        [SerializeField] private Color selectedColor = Color.white;


        private void Reset()
        {
            nameGUI = GetComponent<TMP_Text>();
            if (nameGUI == null)
            {
                nameGUI = GetComponentInChildren<TMP_Text>();
            }
        }

        protected override void DrawItem(DisplayableObject item)
        {
            if (nameGUI != null)
            {
                nameGUI.text = item.Name;
            }
            if (icon != null)
            {
                icon.sprite = item.Icon;
            }
        }
        public override void SetHighlight(bool active)
        {
            base.SetHighlight(active);
            if (background != null)
            {
                background.color = active ? selectedColor : backgroundColor;
            }
        }
    }
}