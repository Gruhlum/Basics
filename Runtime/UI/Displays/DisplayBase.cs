using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class DisplayBase<T> : MonoBehaviour
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


        public virtual void SetItem(T item)
        {
            this.Item = item;
            DrawItem(item);
        }

        protected abstract void DrawItem(T item);
    }
}