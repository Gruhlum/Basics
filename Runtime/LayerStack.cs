using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class LayerStack<T>
    {
        private int activeElementsIndex = -1;

        public T ActiveItem
        {
            get
            {
                return activeItem;
            }
            private set
            {
                activeItem = value;
            }
        }
        private T activeItem;


        private Dictionary<int, List<T>> items = new Dictionary<int, List<T>>();

        public event Action<T> OnActiveItemChanged;


        public LayerStack()
        {
        }

        public void Clear()
        {
            items.Clear();
        }
        /// <summary>
        /// Adds an item to the stack.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index">The items with highest index will be shown first. Needs to be >= 0</param>
        public void Add(T item, int index)
        {
            if (items.TryGetValue(index, out List<T> itemList))
            {
                itemList.Add(item);
            }
            else items.Add(index, new List<T>() { item });
            UpdateActiveItem();
        }
        public void Remove(T item, int index)
        {
            if (items.TryGetValue(index, out List<T> itemList))
            {
                itemList.Remove(item);
                if (itemList.Count <= 0)
                {
                    items.Remove(index);
                }
            }
            UpdateActiveItem();
        }
        private void UpdateActiveItem()
        {
            T result = FindActiveItem();

            if (result == null)
            {
                OnActiveItemChanged?.Invoke(default);
                return;
            }

            if (ActiveItem != null && ActiveItem.Equals(result))
            {
                return;
            }

            ActiveItem = result;
            OnActiveItemChanged?.Invoke(ActiveItem);
        }

        private T FindActiveItem()
        {
            if (items == null || items.Count <= 0)
            {
                return default;
            }
            int highestKey = items.Keys.Max();

            if (!items.TryGetValue(highestKey, out List<T> itemList))
            {
                return default;
            }

            if (itemList.Count <= 0)
            {
                return default;
            }

            activeElementsIndex = GetActiveItemIndex();

            return itemList[activeElementsIndex];
        }

        private int GetActiveItemIndex()
        {
            return 0;
        }
    }
}