using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class DisplayableObject : ScriptableObject, IDisplayable
    {
        public virtual Sprite Icon
        {
            get
            {
                return icon;
            }
        }
        [SerializeField] private Sprite icon = default;

        public virtual string Name
        {
            get
            {
                return name;
            }
        }      
    }
}