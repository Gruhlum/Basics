using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public abstract class BaseStack<T>
    {
        public float RotationDuration
        {
            get
            {
                return rotationDuration;
            }
            set
            {
                rotationDuration = value;
            }
        }
        private float rotationDuration;

        private bool rotateItems;

        private float rotationTimer;
        private int rotationIndex;

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


        private List<T>[] items;

        public event Action<T> OnActiveItemChanged;


        public BaseStack(int layers)
        {
            items = new List<T>[layers];
        }
        public BaseStack(int layers, float rotationTime) : this(layers)
        {
            if (rotationTime <= 0)
            {
                Debug.Log("rotationTime needs to be greater than 0");
                return;
            }
            RotationDuration = rotationTime;
            rotateItems = true;
        }

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
        public void AdvanceTime(float increase)
        {
            if (!rotateItems)
            {
                return;
            }
            if (activeElementsIndex <= -1)
            {
                return;
            }
            if (activeLayerIndex <= -1)
            {
                return;
            }
            if (items[activeLayerIndex].Count <= 1)
            {
                return;
            }

            rotationTimer += increase;
            if (rotationTimer >= RotationDuration)
            {
                rotationTimer = 0;
                rotationIndex++;
                if (rotationIndex >= items[activeLayerIndex].Count)
                {
                    rotationIndex = 0;
                }
                UpdateActiveItem();
            }
        }
        public void AddItem(T item, int index)
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
            if (items[index] == null)
            {
                items[index] = new List<T>();
            }
            items[index].Add(item);
            UpdateActiveItem();
        }
        public void RemoveItem(T item, int index)
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
                if (CompareItems(list[i], item))
                {
                    return i;
                }
            }
            return -1;
        }
        protected abstract bool CompareItems(T item1, T item2);
        private void UpdateActiveItem()
        {
            activeLayerIndex = GetTopLayerIndex();

            if (activeLayerIndex <= -1 || activeElementsIndex <= -1)
            {
                Debug.Log("Empty stack!");
                return;
            }

            T result = items[activeLayerIndex][activeElementsIndex];
            if (CompareItems(ActiveItem, result))
            {
                return;
            }

            ActiveItem = result;
            OnActiveItemChanged?.Invoke(ActiveItem);
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