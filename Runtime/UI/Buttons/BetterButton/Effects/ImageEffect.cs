using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    [System.Serializable]
    public class ImageEffect : ButtonEffect
    {
        [SerializeField] private Image img = default;

        [Space, SerializeField] private bool changeSprite = default;
        [SerializeField, DrawIf(nameof(changeSprite), true)] private Sprite sprite = default;
        [Space, SerializeField] private bool changeColor = default;
        [SerializeField, DrawIf(nameof(changeColor), true)] private Color targetColor = Color.white;

        private Color? oldColor;
        private Sprite oldSprite;


        public override void OnValidate(GameObject go)
        {
            base.OnValidate(go);
            if (img == null)
            {
                img = go.GetComponent<Image>();
            }
            if (img == null)
            {
                img = go.GetComponentInChildren<Image>();
            }
        }

        public override void Apply()
        {
            if (changeColor)
            {
                oldColor = img.color;
                img.color = targetColor;
            }
            if (changeSprite)
            {
                oldSprite = img.sprite;
                img.sprite = sprite;
            }
        }

        public override void Remove()
        {
            if (oldColor != null)
            {
                img.color = oldColor.Value;
                oldColor = null;
            }
            if (oldSprite != null)
            {
                img.sprite = oldSprite;
                oldSprite = null;
            }
        }
    }
}