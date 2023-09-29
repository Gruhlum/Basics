using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
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
        [SerializeField][HideInInspector] private T item = default;

        [SerializeField][HideInInspector] private DisplayController<T> dc;

        public virtual void Setup(T item, DisplayController<T> dc)
        {
            this.Item = item;
            this.dc = dc;
        }
        public void OnDisplayClicked()
        {
            dc.OnDisplayClicked(this);
        }
    }
}