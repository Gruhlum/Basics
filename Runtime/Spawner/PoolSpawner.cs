using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Base class for spawners that utilize a pool of inactive instances.
    /// </summary>
    [System.Serializable]
    public abstract class PoolSpawner<T> : BasicSpawner<T> where T : Component
    {
        protected abstract List<T> Instances
        {
            get;
        }

        /// <summary>
        /// Either returns a deactivated instance or instantiates a new one.
        /// </summary>
        /// <param name="activate">Sets the gameObject active</param>
        public virtual T Spawn(bool activate)
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

            T instance = Instances.Find(x => !x.gameObject.activeSelf);

            if (instance == null)
            {
                instance = Object.Instantiate(Prefab, Parent);
                Instances.Add(instance);
            }

            if (activate)
            {
                instance.gameObject.SetActive(true);
            }

            return instance;
        }
        /// <summary>
        /// Either returns a deactivated object or instantiates a new one.
        /// </summary>
        public override T Spawn()
        {
            return Spawn(true);
        }

        /// <returns>total count of all instances</returns>
        public int TotalInstances()
        {
            return Instances.Count;
        }
        /// <returns>total count of all active instances</returns>
        public int TotalActiveInstances()
        {
            return Instances.FindAll(x => x.gameObject.activeSelf).Count;
        }

        /// <returns>All instances.</returns>
        public IEnumerable<T> GetInstances()
        {
            return Instances;
        }

        /// <returns>All instances that are active.</returns>
        public IEnumerable<T> GetActiveInstances()
        {
            List<T> results = Instances.FindAll(x => x.gameObject.activeSelf);
            return results;
        }
        /// <summary>
        /// Removes a specific instance from the internal list.
        /// </summary>
        /// <param name="t">item to be removed</param>
        public void Remove(T t)
        {
            Instances.Remove(t);
        }
        /// <summary>
        /// Looks for any instances on the parent of the same type and adds them to the internal list.
        /// </summary>
        public void FindInstancesInChildren()
        {
            AddInstances(Parent.GetComponentsInChildren<T>().ToList());
        }
        /// <summary>
        /// Adds instances from one list to this list. Used when merging two Spawner.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="setParent"></param>
        public void AddInstances(List<T> list, bool setParent = false)
        {
            Instances.AddRange(list);
            if (setParent)
            {
                list.ForEach(x => x.transform.SetParent(Parent));
            }
        }

        /// <summary>
        /// Deactivates all instances.
        /// </summary>
        public virtual void DeactivateAll()
        {
            if (Application.isPlaying == false)
            {
                DestroyAll();
            }
            foreach (var instance in Instances)
            {
                if (instance != null)
                {
                    instance.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Checks for any destroyed instances and removes them from the list.
        /// </summary>
        public void RemoveEmptyElements()
        {
            for (int i = Instances.Count - 1; i >= 0; i--)
            {
                if (Instances[i] == null)
                {
                    Instances.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Destroys any inactive instances;
        /// </summary>
        public void DestroyUnused()
        {
            for (int i = Instances.Count - 1; i >= 0; i--)
            {
                if (!Instances[i].gameObject.activeSelf)
                {
                    Object.DestroyImmediate(Instances[i].gameObject);
                    Instances.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Destroys every instance.
        /// </summary>
        public override void DestroyAll()
        {
            base.DestroyAll();
            Instances.Clear();
        }
    }
}