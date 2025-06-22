using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Simplifies the instantiating of objects.
    /// </summary>
    /// <typeparam name="T">Any type that inherits from Component</typeparam>
    [System.Serializable]
    public class BasicSpawner<T> where T : Component
    {
        /// <summary>
        /// Prefab that will be instantiated.
        /// </summary>
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
        [SerializeField, Tooltip("Prefab that will be instantiated")] private T prefab = default;

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
        [SerializeField, Tooltip("Optional parent for the instantiated object")] private Transform parent = default;


        public virtual T Spawn()
        {
            if (prefab == null)
            {
                Debug.LogWarning("Prefab is not assigned!");
                return null;
            }

            T behaviour = Object.Instantiate(prefab, parent);
            return behaviour;
        }
        /// <summary>
        /// Instantiates and returns a new object.
        /// </summary>

        /// <summary>
        /// Destroys all instances of the same type that are children of the parent.
        /// </summary>
        public virtual void DestroyAll()
        {
            if (parent == null)
            {
                return;
            }
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                T t = parent.GetChild(i).GetComponent<T>();
                if (t != null)
                {
                    if (!Application.isPlaying)
                    {
                        Object.DestroyImmediate(t.gameObject);
                    }
                    else Object.Destroy(t.gameObject);
                }
            }
        }
    }
}