using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class DisplayController<T> : MonoBehaviour
    {
        [SerializeField] protected Spawner<Display<T>> displaySpawner = default;

        [SerializeField] protected List<T> items = new List<T>();

        public event Action<Display<T>> OnDisplayClicked;

        [ContextMenu("Generate Displays")]
        public virtual void Setup()
        {
            DisplayItems();
        }

        private void DisplayItems()
        {
            displaySpawner.DeactivateAll();
            foreach (var item in items)
            {
                displaySpawner.Spawn().Setup(item, this);
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
            return displaySpawner.GetActiveBehaviours().ToList();
        }
        public void SetItems(List<T> items, bool display = true)
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
        public void AddItem(T item, int index = 0, bool display = true)
        {
            items.Insert(index, item);
            if (display)
            {
                displaySpawner.Spawn().Setup(item, this);
            }           
        }
        public void RemoveItem(T item)
        {
            items.Remove(item);
        }
    }
}