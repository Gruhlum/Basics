using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class Spawner<T> where T : Component
    {
        public T Prefab
        {
            get
            {
                return prefab;
            }
            set
            {
                prefab = value;
            }
        }
        [SerializeField] private T prefab = default;

        public Transform Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }
        [SerializeField] private Transform parent = default;

        private readonly List<T> behaviours = new List<T>();


        public T Spawn()
        {
            if (prefab == null)
            {
                Debug.LogWarning("Prefab is not assigned!");
                return null;
            }
            if (!Application.isPlaying)
            {
                RemoveEmptyElements();
            }
            if (behaviours.Any(x => x.gameObject.activeSelf == false))
            {
                T behaviour = behaviours.Find(x => x.gameObject.activeSelf == false);
                behaviour.gameObject.SetActive(true);
                return behaviour;
            }
            else
            {
                T behaviour = Object.Instantiate(prefab, parent);
                behaviours.Add(behaviour);
                return behaviour;
            }
        }

        public int TotalBehaviours()
        {
            return behaviours.Count();
        }
        public int TotalActiveBehaviours()
        {
            return behaviours.FindAll(x => x.gameObject.activeSelf).Count;
        }
        public IEnumerable<T> GetBehaviours()
        {
            return behaviours;
        }
        public IEnumerable<T> GetActiveBehaviours()
        {
            List<T> results = behaviours.FindAll(x => x.gameObject.activeSelf);
            return results;
        }

        public void AddInstances(List<T> list, bool setParent = false)
        {
            behaviours.AddRange(list);
            if (setParent)
            {
                list.ForEach(x => x.transform.SetParent(Parent));
            }
        }

        public void DeactivateAll()
        {
            if (Application.isPlaying == false)
            {
                RemoveEmptyElements();
            }
            foreach (var behaviour in behaviours)
            {
                behaviour.gameObject.SetActive(false);
            }
        }

        public void RemoveEmptyElements()
        {
            for (int i = behaviours.Count - 1; i >= 0; i--)
            {
                if (behaviours[i] == null)
                {
                    behaviours.RemoveAt(i);
                }
            }
        }
    }
}