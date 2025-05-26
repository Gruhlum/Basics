using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public interface ILinkListener
    {
        public event Action<string> OnLinkHover;
        public event Action OnHoverStopped;
    }
}