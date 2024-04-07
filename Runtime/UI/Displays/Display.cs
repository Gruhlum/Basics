using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class Display<T> : MonoBehaviour where T : class
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
        [SerializeField][HideInInspector] private T item = default;

        [SerializeField][HideInInspector] protected DisplayController<T> displayC;

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
            this.displayC = dc;
        }
        protected abstract void DrawItem(T item);

        public virtual void SetItem(T item)
        {
            this.Item = item;
            DrawItem(item);
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