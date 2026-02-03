using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class SerializableCollection<T, I> : ScriptableObjectCollectionBase<I> where T : Object where I : class
    {
        [SerializeField] private List<T> items;

        protected override List<I> Items
        {
            get
            {
                return this.castItems;
            }
            set
            {
                castItems = value;
            }
        }
        private List<I> castItems;


        private void OnValidate()
        {
            Items = new List<I>();

            if (items == null || items.Count <= 0)
            {
                return;
            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] != null && items[i] is not I)
                {
                    Debug.Log($"{items[i].name} needs to be of type {nameof(I)}");
                    items.RemoveAt(i);
                }
            }

            foreach (T item in items)
            {
                Items.Add(item as I);
            }
        }
    }
}