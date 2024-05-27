using System;
using System.Collections;
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


        private readonly List<TypeList> typeLists = new List<TypeList>();

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

            T instance = FindDeactivatedInstance<T>(prefab);

            if (instance == null)
            {
                instance = CreateCopy(prefab);
            }

            instance.gameObject.SetActive(true);

            return instance;
        }
        /// <returns>All instances.</returns>
        public List<T> GetActiveInstances<T>() where T : MonoBehaviour
        {
            List<T> results = new List<T>();
            List<TypeList> lists = typeLists.FindAll(x => x.prefab is T);
            foreach (var list in lists)
            {
                foreach (var item in list.instances)
                {
                    if (item.gameObject.activeInHierarchy)
                    {
                        results.Add(item as T);
                    }                   
                }                
            }
            return results;
        }

        /// <returns>All instances that are active.</returns>
        public List<Component> GetAllActiveInstances()
        {
            List<Component> results = new List<Component>();
            foreach (var list in typeLists)
            {
                results.AddRange(list.instances);
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
            for (int i = typeLists.Count - 1; i >= 0; i--)
            {
                for (int j = typeLists[i].instances.Count - 1; j >= 0; j--)
                {
                    typeLists[i].instances[j].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Destroys every instance.
        /// </summary>
        public void DestroyAll()
        {
            for (int i = typeLists.Count - 1; i >= 0; i--)
            {
                for (int j = typeLists[i].instances.Count - 1; j >= 0; j--)
                {
                    UnityEngine.Object.Destroy(typeLists[i].instances[j].gameObject);
                }
            }
        }
        private TypeList GetTypeList(Component prefab)
        {
            return typeLists.Find(x => x.prefab == prefab);
        }
        private T FindDeactivatedInstance<T>(Component prefab) where T : Component
        {
            TypeList typeList = GetTypeList(prefab);
            if (typeList == null)
            {
                return null;
            }
            T instance = typeList.instances.Find(x => !x.gameObject.activeInHierarchy) as T;
            return instance;
        }

        private T CreateCopy<T>(T prefab) where T : Component
        {
            if (prefab == null)
            {
                return null;
            }
            T clone = UnityEngine.Object.Instantiate(prefab);
            clone.transform.SetParent(Parent);

            TypeList typeList = GetTypeList(prefab);

            if (typeList == null)
            {
                typeList = new TypeList(prefab);
                typeLists.Add(typeList);
            }
            typeList.instances.Add(clone);
            return clone;
        }

       

        public class TypeList
        {
            public Component prefab;
            public List<Component> instances;

            public TypeList(Component prefab)
            {
                this.prefab = prefab;
                instances = new List<Component>();
            }
        }
    }
}