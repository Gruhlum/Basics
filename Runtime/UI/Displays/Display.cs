using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class Display<T> : MonoBehaviour
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

#if UNITY_EDITOR
        [ContextMenu("Draw Item")]
        public void DrawItem()
        {
            if (item != null)
            {
                DrawItem(item);
            }
        }
#endif

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