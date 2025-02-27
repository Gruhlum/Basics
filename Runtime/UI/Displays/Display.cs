using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class Display<T> : DisplayBase<T>
    {
        [SerializeField] protected DisplayControllerBase<T> displayC;

        public bool IsHighlighted
        {
            get
            {
                return isHighlighted;
            }
            private set
            {
                isHighlighted = value;
            }
        }
        private bool isHighlighted;

        public void Setup(T item, DisplayController<T> dc)
        {
            SetItem(item);
            SetController(dc);
        }
        public void SetController(DisplayController<T> dc)
        {
            this.displayC = dc;
        }
        public virtual void OnDisplayClicked()
        {
            displayC.DisplayClicked(this);
        }
        public virtual void SetHighlight(bool active)
        {
            IsHighlighted = active;
        }
    }
}