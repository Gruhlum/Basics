using System;
using System.Collections.Generic;
using System.Linq;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class LayerStack<T>
    {
        private int activeElementsIndex = -1;
        private int activeLayer;

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

        public void ClearLayer(int layer)
        {
            if (items.Remove(layer))
            {
                FindActiveLayer();
                UpdateActiveItem();
            }
        }
        public void ClearAll()
        {
            items.Clear();
            UpdateActiveItem();
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
            else
            {
                items.Add(index, new List<T>() { item });
                if (activeLayer < index)
                {
                    activeLayer = index;
                }
            }
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
                    FindActiveLayer();
                }
            }
            UpdateActiveItem();
        }
        private void FindActiveLayer()
        {
            int? highestKey = default;

            foreach (var item in items)
            {
                if (item.Value.Count <= 0)
                {
                    continue;
                }
                if (!highestKey.HasValue || item.Key > highestKey)
                {
                    highestKey = item.Key;
                }
            }
            activeLayer = highestKey.Value;
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

            if (!items.TryGetValue(activeLayer, out List<T> results))
            {
                return default;
            }

            if (results.Count <= 0)
            {
                return default;
            }

            activeElementsIndex = GetActiveItemIndex();

            return GetActiveItem(results);
        }
        protected virtual T GetActiveItem(List<T> results)
        {
            return results[activeElementsIndex];
        }

        private int GetActiveItemIndex()
        {
            return 0;
        }
    }
}