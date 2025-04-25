using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    [System.Serializable]
    public class ImageChangeEffect : ImageEffect
    {
        [SerializeField] private Sprite sprite = default;
        
        private Sprite oldSprite;


        

        public override void Apply()
        {
            oldSprite = img.sprite;
            img.sprite = sprite;
        }

        public override void Remove()
        {
            img.sprite = oldSprite;
            oldSprite = null;
        }
    }
}