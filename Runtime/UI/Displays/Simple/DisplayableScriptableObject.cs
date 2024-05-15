using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class DisplayableScriptableObject : ScriptableObject, IDisplayable
    {
        public abstract Sprite Sprite
        {
            get;
        }

        public virtual string Name
        {
            get
            {
                return name;
            }
        }
    }
}