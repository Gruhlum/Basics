using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class Spawner<T> : BasicSpawner<T> where T : Component
    {       
        private readonly List<T> behaviours = new List<T>();

        public override T Spawn()
        {
            if (Prefab == null)
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
                T behaviour = Object.Instantiate(Prefab, Parent);
                behaviours.Add(behaviour);
                return behaviour;
            }
        }

        public void Remove(T t)
        {
            behaviours.Remove(t);
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
        public void FindBehavioursInChildren()
        {
            AddInstances(Parent.GetComponentsInChildren<T>().ToList());
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
                TryDestroyAll();
            }
            foreach (var behaviour in behaviours)
            {
                if (behaviour != null)
                {
                    behaviour.gameObject.SetActive(false);
                }              
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
        public void DestroyUnused()
        {
            for (int i = behaviours.Count - 1; i >= 0; i--)
            {
                if (behaviours[i].gameObject.activeSelf == false)
                {
                    Object.DestroyImmediate(behaviours[i].gameObject);
                }
            }
        }
        public override void TryDestroyAll()
        {
            base.TryDestroyAll();
            behaviours.Clear();
        }
    }
}