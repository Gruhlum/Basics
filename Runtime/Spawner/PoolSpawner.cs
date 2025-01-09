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
    public abstract class PoolSpawner<T> : BasicSpawner<T>, IEnumerable<T> where T : Component
    {
        protected abstract HashSet<T> Instances
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

            T instance = GetEmptyInstance();

            if (activate)
            {
                instance.gameObject.SetActive(true);
            }

            return instance;
        }
        /// <summary>
        /// Either returns a deactivated object or instantiates a new one.
        /// </summary>
        public sealed override T Spawn()
        {
            return Spawn(true);
        }
        protected virtual T GetEmptyInstance()
        {
            T instance = Instances.First(x => !x.gameObject.activeSelf);
            if (instance != null)
            {
                return instance;
            }
            return CreateNewInstance();
        }

        protected T CreateNewInstance()
        {
            T instance = Object.Instantiate(Prefab, Parent);
            Instances.Add(instance);
            return instance;
        }

        /// <returns>total count of all instances</returns>
        public virtual int TotalInstances()
        {
            return Instances.Count;
        }
        /// <returns>total count of all active instances</returns>
        public virtual int TotalActiveInstances()
        {
            return Instances.Count(x => x.gameObject.activeSelf);
        }

        /// <summary>
        /// Removes a specific instance from the collection.
        /// </summary>
        /// <param name="t">item to be removed</param>
        public void Remove(T t)
        {
            Instances.Remove(t);
        }
        /// <summary>
        /// Looks for any instances on the parent of the same type and adds them to the internal list.
        /// </summary>
        protected void FindInstancesInChildren()
        {
            AddInstances(Parent.GetComponentsInChildren<T>().ToHashSet());
        }
        /// <summary>
        /// Adds instances from one list to this list. Used when merging two Spawner.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="setParent"></param>
        public void AddInstances(HashSet<T> set, bool setParent = false)
        {
            Instances.UnionWith(set);
            if (setParent)
            {
                foreach (var item in set)
                {
                    item.transform.SetParent(Parent);
                }
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
        /// Checks for any destroyed instances and removes them from the collection.
        /// </summary>
        public void RemoveEmptyElements()
        {
            Instances.RemoveWhere(x => x == null);
        }

        /// <summary>
        /// Destroys any inactive instances;
        /// </summary>
        public void DestroyUnused()
        {
            List<T> toDestroy = new List<T>();

            foreach (var item in Instances)
            {
                if (!item.gameObject.activeSelf)
                {
                    toDestroy.Add(item);
                }
            }
            for (int i = toDestroy.Count - 1; i >= 0; i--)
            {
                Instances.Remove(toDestroy[i]);
                Object.DestroyImmediate(toDestroy[i]);
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

        public IEnumerator<T> GetEnumerator()
        {
            return Instances.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Instances.GetEnumerator();
        }
    }
}