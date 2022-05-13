using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class StaticSpawner<T> where T : Component
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

        [HideInInspector] private static readonly List<T> behaviours = new List<T>();

        private bool isInit = false;

        public void Init()
        {
            RemoveEmptyElements();
            isInit = true;
        }

        public T Spawn()
        {
            if (isInit == false)
            {
                Debug.LogWarning("Spawner not initialized, call Init() before use.");
                return null;
            }
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
            return behaviours.FindAll(x => x.gameObject.activeInHierarchy).Count;
        }
        public IEnumerable<T> GetBehaviours()
        {
            return behaviours;
        }
        public IEnumerable<T> GetActiveBehaviours()
        {
            List<T> results = behaviours.FindAll(x => x.gameObject.activeInHierarchy);
            return results;
        }

        public void ConsumeInstances(List<T> list)
        {
            behaviours.AddRange(list);
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