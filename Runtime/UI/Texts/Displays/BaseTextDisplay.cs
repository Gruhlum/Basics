using System;

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


        protected override void OnDestroy()
        {
            base.OnDestroy();
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