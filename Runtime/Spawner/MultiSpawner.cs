using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Spawner that generates a separate pool for any new prefab that it is spawning.
    /// Inactivate Instances will be reused before new ones are instantiated.
    /// </summary>
    [System.Serializable]
    public class MultiSpawner
    {
        /// <summary>
        /// Optional parent for the instantiated object.
        /// </summary>
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
        [SerializeField, Tooltip("Prefab that will be instantiated")] private Transform parent = default;


        private Dictionary<Component, HashSet<Component>> instances = new Dictionary<Component, HashSet<Component>>();

        /// <summary>
        /// Either returns a deactivated instance or instantiates a new one.
        /// </summary>
        /// <param name="prefab">Prefab to instantiate.</param>
        /// <returns></returns>
        public T Spawn<T>(T prefab) where T : Component
        {
            if (prefab == null)
            {
                return null;
            }

            T instance = GetEmptyInstance(prefab);
            instance.gameObject.SetActive(true);
            return instance;
        }
        private T GetEmptyInstance<T>(T prefab) where T : Component
        {
            if (instances.TryGetValue(prefab, out HashSet<Component> set))
            {
                if (set.Count == 0)
                {
                    return CreateNewCopy(prefab, set);
                }

                Component result = set.First(x => !x.gameObject.activeSelf);
                if (result != null)
                {
                    return result as T;
                }
                else return CreateNewCopy(prefab, set);
            }
            else return CreateNewCopy(prefab, new HashSet<Component>());
        }
        private T CreateNewCopy<T>(T prefab, HashSet<Component> set) where T : Component
        {
            T instance = UnityEngine.Object.Instantiate(prefab, Parent);
            set.Add(instance);
            return instance;
        }


        /// <returns>All instances.</returns>
        public List<T> GetAllInstances<T>(T prefab) where T : Component
        {
            List<T> results = new List<T>();

            if (instances.TryGetValue(prefab, out HashSet<Component> set))
            {
                foreach (Component component in set)
                {
                    results.Add(component as T);
                }
            }
            return results;
        }
        /// <returns>All instances that are active.</returns>
        public List<T> GetActiveInstances<T>(T prefab) where T : Component
        {
            List<T> results = new List<T>();

            if (instances.TryGetValue(prefab, out HashSet<Component> set))
            {
                foreach (Component component in set)
                {
                    if (component.gameObject.activeSelf)
                    {
                        results.Add(component as T);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Deactivates all instances.
        /// </summary>
        public void DeactivateAll()
        {
            if (!Application.isPlaying)
            {
                DestroyAll();
                return;
            }

            foreach (HashSet<Component> set in instances.Values)
            {
                foreach (Component component in set)
                {
                    component.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Destroys every instance.
        /// </summary>
        public void DestroyAll()
        {
            foreach (HashSet<Component> set in instances.Values)
            {
                foreach (Component component in set)
                {
                    UnityEngine.Object.Destroy(component);
                }
            }
        }
    }
}