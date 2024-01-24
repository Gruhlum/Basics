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

        [SerializeField][HideInInspector] protected DisplayController<T> dc;

        public virtual void Setup(T item, DisplayController<T> dc)
        {
            SetItem(item);
            this.dc = dc;
        }
        public void SetItem(T Item)
        {
            this.Item = Item;
        }
        public virtual void OnDisplayClicked()
        {
            dc.DisplayClicked(this);
        }
    }
}