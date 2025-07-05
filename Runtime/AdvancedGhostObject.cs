using TMPro;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class AdvancedGhostObject : GhostObject
    {
        [SerializeField] private TMP_Text textGUI = default;

        public void Activate(Vector3 pos, Sprite sprite, Color col, string text)
        {
            base.Activate(pos, sprite, col);
            UpdateText(text);
        }
        public void UpdateText(string text)
        {
            textGUI.text = text;
        }
    }
}