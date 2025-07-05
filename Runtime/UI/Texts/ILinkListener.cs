using System;

namespace HexTecGames.Basics.UI
{
    public interface ILinkListener
    {
        event Action<LinkListener, TextData> OnLinkHover;
        event Action<LinkListener> OnHoverStopped;

        bool HasListener
        {
            get;
        }
    }
}