using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class ScriptableObjectCollectionBase<T> : ScriptableObject, IEnumerable<T> where T : class
    {
        protected abstract List<T> Items
        {
            get;
            set;
        }

        public ReadOnlyCollection<T> GetItems()
        {
            return Items.AsReadOnly();
        }


        public bool Contains(T item)
        {
            return Items.Contains(item);
        }
        public T GetItem(int index)
        {
            if (index < 0)
            {
                return null;
            }
            if (index >= Items.Count)
            {
                return null;
            }
            return Items[index];
        }
        public int GetIndex(T t)
        {
            return Items.IndexOf(t);
        }
        public List<T> GetRandomItems(int amount)
        {
            return Items.Random(amount);
        }
        public List<T> GetUniqueRandomItems(int amount)
        {
            return Items.RandomUnique(amount);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}