using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class DisplayControllerBase<T> : MonoBehaviour
    {
        public event Action<Display<T>> OnDisplayClicked;

        public virtual void DisplayClicked(Display<T> display)
        {
            OnDisplayClicked?.Invoke(display);
        }
    }
}