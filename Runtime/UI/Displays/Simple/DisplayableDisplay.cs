using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class DisplayableDisplay : Display<DisplayableScriptableObject>
    {
        [SerializeField] private TMP_Text nameGUI = default;
        [SerializeField] private Image img = default;
        [SerializeField] private Image background = default;

        [SerializeField] private Color backgroundColor = Color.black;
        [SerializeField] private Color selectedColor = Color.black;

        protected override void DrawItem(DisplayableScriptableObject item)
        {
            nameGUI.text = item.Name;
            img.sprite = item.Sprite;
        }
        public override void SetHighlight(bool active)
        {
            base.SetHighlight(active);
            background.color = active ? selectedColor : backgroundColor;
        }
    }
}