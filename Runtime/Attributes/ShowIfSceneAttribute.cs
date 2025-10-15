using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public enum ShowType
    {
        All,
        Scene,
        Assets
    }

    public class DrawIfSceneAttribute : PropertyAttribute 
    {
        public ShowType ShowType { get; private set; }

        public DrawIfSceneAttribute(ShowType showType = ShowType.Scene)
        {
            this.ShowType = showType;
        }

        public bool Show(Object targetObject)
        {
#if UNITY_EDITOR
            if (ShowType == ShowType.All)
            {
                return true;
            }
            if (UnityEditor.EditorUtility.IsPersistent(targetObject) && ShowType == ShowType.Assets)
            {
                return true;
            }
            if (!UnityEditor.EditorUtility.IsPersistent(targetObject) && ShowType == ShowType.Scene)
            {
                return true;
            }
#endif
            return false;
        }
    }
}