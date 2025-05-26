using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class BaseTextDisplay<TDisplay, T> : Display<TDisplay, T>, ILinkListener where TDisplay : Display<TDisplay, T>
    {
        public event Action<string> OnLinkHover;
        public event Action OnHoverStopped;


        protected virtual void OnDestroy()
        {
            OnLinkHover = null;
            OnHoverStopped = null;
        }

        protected void LinkHover(string obj)
        {
            OnLinkHover?.Invoke(obj);
        }
        protected void HoverStopped()
        {
            OnHoverStopped?.Invoke();
        }
    }
}