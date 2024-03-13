using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class SpriteStack : BaseStack<Sprite>
    {
        public SpriteStack(int layers) : base(layers)
        {
        }

        public SpriteStack(int layers, float rotationTime) : base(layers, rotationTime)
        {
        }

        protected override bool CompareItems(Sprite item1, Sprite item2)
        {
            return item1 == item2;
        }
    }
}