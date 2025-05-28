using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class BaseTextDisplay<TDisplay, T> : Display<TDisplay, T>, ILinkListener where TDisplay : Display<TDisplay, T>
    {
        public abstract bool HasListener
        {
            get;
        }

        public event Action<LinkListener, TextData> OnLinkHover;
        public event Action<LinkListener> OnHoverStopped;


        protected virtual void OnDestroy()
        {
            OnLinkHover = null;
            OnHoverStopped = null;
        }

        protected void LinkHover(LinkListener singleDisplay, TextData obj)
        {
            OnLinkHover?.Invoke(singleDisplay, obj);
        }
        protected void HoverStopped(LinkListener singleDisplay)
        {
            OnHoverStopped?.Invoke(singleDisplay);
        }
    }
}