using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class ScriptableObjectCollection<T> : ScriptableObject, IEnumerable<T> where T : ScriptableObject
    {
        [SerializeField] private List<T> items = new List<T>();

        public T GetItem(int index)
        {
            if (index < 0)
            {
                return null;
            }
            if (index >= items.Count)
            {
                return null;
            }
            return items[index];
        }
        public int GetIndex(T t)
        {
            return items.IndexOf(t);
        }
        public List<T> GetRandomItems(int amount)
        {
            return items.Random(amount);
        }
        public List<T> GetUniqueRandomItems(int amount)
        {
            return items.RandomUnique(amount);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}