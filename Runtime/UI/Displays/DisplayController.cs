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
        [SerializeField] protected List<D> displays = default;
        [Space]
        [SerializeField, SerializeReference] protected List<T> items = new List<T>();

        protected virtual void Reset()
        {
            displays = GetComponentsInChildren<D>().ToList();
        }

        protected D SpawnDisplay()
        {
            D display = displaySpawner.Spawn();
            return display;
        }
        public virtual void DisplayItems()
        {
            if (displays != null && displays.Count > 0)
            {
                for (int i = 0; i < displays.Count; i++)
                {
                    displays[i].gameObject.SetActive(true);
                    if (items.Count != 0 && items.Count <= i)
                    {

                    }
                    else SetupDisplay(displays[i], items[i]);
                }
            }
            else
            {
                displaySpawner.DeactivateAll();
                foreach (var item in items)
                {
                    SetupDisplay(SpawnDisplay(), item);
                }
            }
        }
        public void SelectFirstDisplay()
        {
            if (displays == null || displays.Count == 0)
            {
                return;
            }
            displays[0].DisplayClicked();
        }
        public List<T> GetItems()
        {
            var results = new List<T>();
            results.AddRange(items);
            return results;
        }
        public int GetTotalItems()
        {
            return items.Count;
        }
        public List<D> GetDisplays()
        {
            if (displays != null && displays.Count > 0)
            {
                return displays;
            }
            else return displaySpawner.ToList();
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