using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    public abstract class DisplayController<T> : MonoBehaviour
    {
        [SerializeField] protected Spawner<Display<T>> displaySpawner = default;

        [SerializeField] private List<T> items = default;

        [ContextMenu("Generate Displays")]
        public void Setup()
        {
            displaySpawner.DeactivateAll();
            foreach (var item in items)
            {
                displaySpawner.Spawn().Setup(item, this);
            }
        }

        public abstract void OnDisplayClicked(Display<T> display);

        public List<T> GetItems()
        {
            var results = new List<T>();
            results.AddRange(items);
            return results;
        }
        public void SetItems(List<T> items)
        {
            items = new List<T>();
            items.AddRange(items);
        }
        public void AddItem(T item, int index = 0, bool display = true)
        {
            items.Insert(index, item);
            displaySpawner.Spawn().Setup(item, this);
        }
        public void RemoveItem(T item)
        {
            items.Remove(item);
        }
    }
}