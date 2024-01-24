using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    //public enum ControllerType { Spawner, Fixed }
    public abstract class DisplayController<T> : MonoBehaviour where T : class
    {
        //[SerializeField] private ControllerType type = default;

        [SerializeField] protected Spawner<Display<T>> displaySpawner = default;

        [SerializeField] protected List<Display<T>> displays = default;

        [SerializeField] protected List<T> items = new List<T>();

        public event Action<Display<T>> OnDisplayClicked;

        protected Display<T> SpawnDisplay()
        {
            return displaySpawner.Spawn();
        }
        protected virtual void SetupDisplay(T item, Display<T> display)
        {
            display.Setup(item, this);
        }
        protected virtual void DisplayItems()
        {
            if (displays != null && displays.Count > 0)
            {
                for (int i = 0; i < displays.Count; i++)
                {
                    if (items.Count <= i)
                    {
                        SetupDisplay(null, displays[i]);
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

        public virtual void DisplayClicked(Display<T> display)
        {
            OnDisplayClicked?.Invoke(display);
        }

        public List<T> GetItems()
        {
            var results = new List<T>();
            results.AddRange(items);
            return results;
        }
        public List<Display<T>> GetDisplays()
        {
            if (displays != null && displays.Count > 0)
            {
                return displays;
            }
            else return displaySpawner.GetActiveBehaviours().ToList();
        }
        public virtual void SetItems(List<T> items, bool display = true)
        {
            this.items = new List<T>();
            this.items.AddRange(items);
            if (display)
            {
                DisplayItems();
            }
        }
        public void ClearItems(bool display = true)
        {
            items.Clear();
            if (display)
            {
                displaySpawner.DeactivateAll();
            }
        }
        public virtual void AddItem(T item, int index = 0, bool display = true)
        {
            items.Insert(index, item);
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