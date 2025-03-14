using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class Display<D, T> : MonoBehaviour where D : Display<D, T>
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

        public event Action<D> OnDisplayClicked;
        public event Action<D> OnDeactivated;

        public virtual void SetItem(T item)
        {
            this.Item = item;
            DrawItem(item);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            OnDeactivated?.Invoke(this as D);
        }

        protected abstract void DrawItem(T item);
        public virtual void DisplayClicked()
        {
            OnDisplayClicked?.Invoke(this as D);
        }
        public virtual void SetHighlight(bool active)
        {
            IsHighlighted = active;
        }
    }
}