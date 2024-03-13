using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class BasicSpawner<T> where T : Component
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
        public virtual void TryDestroyAll()
        {
            if (parent == null)
            {
                Debug.LogWarning("Can't find objects without setting a parent");
                return;
            }
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                T t = parent.GetChild(i).GetComponent<T>();
                if (t != null)
                {
                    if (Application.isPlaying)
                    {
                        Object.Destroy(t.gameObject);
                    }
                    else Object.DestroyImmediate(t.gameObject);
                }
            }
        }
    }
}