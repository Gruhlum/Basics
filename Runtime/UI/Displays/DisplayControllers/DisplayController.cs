using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class DisplayController<D, T> : DisplayControllerBase<D, T> where D : Display<D, T>
    {
        [SerializeField] protected Spawner<D> displaySpawner = default;
        [Space]
        [SerializeField, SerializeReference] protected List<T> items = new List<T>();

        public int TotalItems
        {
            get
            {
                return items.Count;
            }
        }

        protected D SpawnDisplay()
        {
            D display = displaySpawner.Spawn(false);
            return display;
        }
        public virtual void DisplayItems()
        {
            displaySpawner.DeactivateAll();
            foreach (var item in items)
            {
                SetupDisplay(SpawnDisplay(), item);
            }
        }
        public void SelectFirstDisplay()
        {
            if (displaySpawner.TotalActiveInstances() <= 0)
            {
                return;
            }
            displaySpawner.GetInstances().First().DisplayClicked();
        }
        public List<T> GetItems()
        {
            var results = new List<T>();
            results.AddRange(items);
            return results;
        }
        public List<D> GetDisplays()
        {
            return displaySpawner.ToList();
        }
        public virtual void SetItems(List<T> items, bool display = true)
        {
            if (items == null)
            {
                ClearItems(display);
                return;
            }
            this.items = items;

            if (display)
            {
                DisplayItems();
            }
        }
        public virtual void ClearItems(bool display = true)
        {
            items.Clear();
            if (display)
            {
                DisplayItems();
            }
        }
        public virtual void InsertItem(T item, int index = 0, bool display = true)
        {
            items.Insert(index, item);
            if (display)
            {
                DisplayItems();
            }
        }
        public virtual void AddItem(T item, bool display = true)
        {
            items.Add(item);
            if (display)
            {
                DisplayItems();
            }
        }
        public void RemoveItem(int index, bool display = true)
        {
            if (index < 0 || index >= items.Count)
            {
                return;
            }
            T item = items[index];
            RemoveItem(item, display);
        }
        public virtual void RemoveItem(T item, bool display = true)
        {
            items.Remove(item);
            if (display)
            {
                DisplayItems();
            }
        }
    }
}