using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class Display<TDisplay, T> : MonoBehaviour where TDisplay : Display<TDisplay, T>
    {
        public T Item
        {
            get
            {
                return item;
            }
            private set
            {
                item = value;
            }
        }
        [SerializeField] private T item = default;
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

        public event Action<TDisplay> OnDisplayClicked;
        public event Action<TDisplay> OnDeactivated;

        protected virtual void OnDisable()
        {
            OnDeactivated?.Invoke(this as TDisplay);
        }

        public virtual void SetItem(T item, bool activate = true)
        {
            this.Item = item;
            DrawItem(item);
            if (activate)
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        protected abstract void DrawItem(T item);
        public virtual void DisplayClicked()
        {
            OnDisplayClicked?.Invoke(this as TDisplay);
        }
        public virtual void SetHighlight(bool active)
        {
            IsHighlighted = active;
        }
    }
}