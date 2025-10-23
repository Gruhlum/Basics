using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class LinkedVectorAttribute : PropertyAttribute
    {
        public bool LinkAxesByDefault;

        public LinkedVectorAttribute(bool linkAxesByDefault = true)
        {
            LinkAxesByDefault = linkAxesByDefault;
        }
    }
}