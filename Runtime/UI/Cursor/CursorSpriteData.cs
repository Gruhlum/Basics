using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class CursorSpriteData
    {
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            private set
            {
                texture = value;
            }
        }
        [SerializeField] private Texture2D texture = default;

        public Vector2 Offset
        {
            get
            {
                return offset;
            }
            private set
            {
                offset = value;
            }
        }
        [Tooltip("The offset from the top left of the texture to use as the target point.")]
        [SerializeField] private Vector2 offset = default;
    }
}