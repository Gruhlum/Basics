using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class DisplayController<T> : DisplayControllerBase<T>
    {
        [SerializeField] protected Spawner<Display<T>> displaySpawner = default;
        [SerializeField] protected List<Display<T>> displays = default;
        [Space]
        [SerializeField, SerializeReference] protected List<T> items = new List<T>();
       
        protected virtual void Reset()
        {
            displays = GetComponentsInChildren<Display<T>>().ToList();
        }

        protected Display<T> SpawnDisplay()
        {
            Display<T> display = displaySpawner.Spawn();
            return display;
        }
        protected virtual void SetupDisplay(T item, Display<T> display)
        {
            display.Setup(item, this);
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
                        displays[i].SetController(this);
                    }
                    else SetupDisplay(items[i], displays[i]);
                }
            }
            else
            {
                displaySpawner.DeactivateAll();
                foreach (var item in items)
                {
                    SetupDisplay(item, SpawnDisplay());
                }
            }
        }       
        public void SelectFirstDisplay()
        {
            if (displays == null || displays.Count == 0)
            {
                return;
            }
            displays[0].OnDisplayClicked();
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
        public List<Display<T>> GetDisplays()
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
            this.items = new List<T>();
            this.items.AddRange(items);
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
                displaySpawner.DeactivateAll();
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