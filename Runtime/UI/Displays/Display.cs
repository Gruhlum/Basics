using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        protected virtual void OnDestroy()
        {
            RemoveEvents(this.Item);
        }

        public virtual void SetItem(T item, bool activate = true)
        {
            if (this.Item != null)
            {
                RemoveEvents(this.Item);
            }

            this.Item = item;

            if (this.Item != null)
            {
                AddEvents(this.Item);
            }

            DrawItem(item);
            if (activate)
            {
                gameObject.SetActive(true);
            }
        }

        protected virtual void AddEvents(T item)
        {
        }
        protected virtual void RemoveEvents(T item)
        { }

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