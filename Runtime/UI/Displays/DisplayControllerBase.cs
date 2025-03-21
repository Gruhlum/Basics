using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class DisplayControllerBase<D, T> : MonoBehaviour where D : Display<D, T>
    {
        public event Action<D> OnDisplayClicked;
        public event Action<D> OnDisplayDeactivated;

        protected virtual void SetupDisplay(D display, T item)
        {
            display.SetItem(item);
            SubscribeEvents(display);
        }
        public virtual void SubscribeEvents(D display)
        {
            if (display == null)
            {
                return;
            }
            display.OnDisplayClicked += Display_OnDisplayClicked;
            display.OnDeactivated += Display_OnDeactivated;
        }

        public virtual void UnsubscribeEvents(D display)
        {
            if (display == null)
            {
                return;
            }
            display.OnDisplayClicked -= Display_OnDisplayClicked;
            display.OnDeactivated -= Display_OnDeactivated;
        }
        protected virtual void Display_OnDisplayClicked(D display)
        {
            OnDisplayClicked?.Invoke(display);
        }
        protected virtual void Display_OnDeactivated(D display)
        {
            UnsubscribeEvents(display);
            OnDisplayDeactivated?.Invoke(display);
        }
    }
}