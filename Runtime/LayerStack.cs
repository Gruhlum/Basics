using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class LayerStack<T>
    {
        //public float RotationDuration
        //{
        //    get
        //    {
        //        return rotationDuration;
        //    }
        //    set
        //    {
        //        rotationDuration = value;
        //    }
        //}
        //private float rotationDuration;

        //private bool rotateItems;

        //private float rotationTimer;
        //private int rotationIndex;

        private int activeElementsIndex = -1;
        private int activeLayerIndex = -1;

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


        private List<T>[] items = new List<T>[4];

        public event Action<T> OnActiveItemChanged;


        public LayerStack()
        {

        }
        //public LayerStack(int layers, float rotationTime) : this(layers)
        //{
        //    if (rotationTime <= 0)
        //    {
        //        Debug.Log("rotationTime needs to be greater than 0");
        //        return;
        //    }
        //    RotationDuration = rotationTime;
        //    rotateItems = true;
        //}

        public void Clear()
        {
            foreach (var list in items)
            {
                if (list != null)
                {
                    list.Clear();
                }
            }
        }
        //public void AdvanceTime(float increase)
        //{
        //    if (!rotateItems)
        //    {
        //        return;
        //    }
        //    if (activeElementsIndex <= -1)
        //    {
        //        return;
        //    }
        //    if (activeLayerIndex <= -1)
        //    {
        //        return;
        //    }
        //    if (items[activeLayerIndex].Count <= 1)
        //    {
        //        return;
        //    }

        //    rotationTimer += increase;
        //    if (rotationTimer >= RotationDuration)
        //    {
        //        rotationTimer = 0;
        //        rotationIndex++;
        //        if (rotationIndex >= items[activeLayerIndex].Count)
        //        {
        //            rotationIndex = 0;
        //        }
        //        UpdateActiveItem();
        //    }
        //}

        /// <summary>
        /// Adds an item to the stack.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index">The items with highest index will be shown first. Needs to be >= 0</param>
        public void Add(T item, int index)
        {
            if (index < 0)
            {
                Debug.Log("index is Negative!");
                return;
            }
            if (index >= items.Length)
            {
                Array.Resize(ref items, index + 1);
            }
            if (items[index] == null)
            {
                items[index] = new List<T>();
            }
            items[index].Add(item);
            UpdateActiveItem();
        }
        public void Remove(T item, int index)
        {
            if (index < 0)
            {
                Debug.Log("index is Negative!");
                return;
            }
            if (index >= items.Length)
            {
                Debug.Log("index out of Range!");
                return;
            }
            int listIndex = FindIndex(item, items[index]);
            if (listIndex < 0)
            {
                return;
            }
            items[index].RemoveAt(listIndex);
            UpdateActiveItem();
        }
        protected int FindIndex(T item, List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }
        private void UpdateActiveItem()
        {
            activeLayerIndex = GetTopLayerIndex();

            if (activeLayerIndex <= -1)
            {
                Debug.Log("Empty stack!");
                return;
            }

            activeElementsIndex = GetActiveItemIndex();

            T result = items[activeLayerIndex][activeElementsIndex];
            if (ActiveItem != null && ActiveItem.Equals(result))
            {
                return;
            }

            ActiveItem = result;
            OnActiveItemChanged?.Invoke(ActiveItem);
        }
        private int GetActiveItemIndex()
        {
            return 0;
        }
        private int GetTopLayerIndex()
        {
            for (int i = items.Length - 1; i >= 0; i--)
            {
                if (items[i] != null && items[i].Count > 0)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}