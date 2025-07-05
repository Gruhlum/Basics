using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    [System.Serializable]
    public class TextChangeEffect : TextEffect
    {
        [SerializeField] private string newText = default;

        private string oldText;


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