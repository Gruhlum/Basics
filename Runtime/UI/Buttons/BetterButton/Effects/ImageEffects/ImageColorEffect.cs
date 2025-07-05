using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    [System.Serializable]
    public class ImageColorEffect : ImageEffect
    {
        [SerializeField] private Color targetColor = Color.white;

        private Color? oldColor;

        public override void Apply()
        {
            oldColor = img.color;
            img.color = targetColor;

        }

        public override void Remove()
        {
            if (oldColor != null)
            {
                img.color = oldColor.Value;
            }
        }
    }
}