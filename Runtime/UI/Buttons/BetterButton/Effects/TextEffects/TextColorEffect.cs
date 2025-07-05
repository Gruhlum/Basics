using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    [System.Serializable]
    public class TextColorEffect : TextEffect
    {
        [SerializeField] private Color color = Color.white;

        private Color oldColor;

        public override void Apply()
        {
            oldColor = textGUI.color;
            textGUI.color = color;
        }

        public override void Remove()
        {
            textGUI.color = oldColor;
        }
    }
}