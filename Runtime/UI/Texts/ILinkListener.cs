using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public interface ILinkListener
    {
        public event Action<LinkListener, TextData> OnLinkHover;
        public event Action<LinkListener> OnHoverStopped;

        public bool HasListener
        {
            get;
        }
    }
}