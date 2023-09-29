using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class MultiSpawner
    {
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


        private readonly List<TypeList> typeLists = new List<TypeList>();


        private TypeList GetTypeList(MonoBehaviour prefab)
        {
            return typeLists.Find(x => x.prefab == prefab);
        }

        public List<T> GetActiveItems<T>() where T : MonoBehaviour
        {
            List<T> results = new List<T>();
            List<TypeList> lists = typeLists.FindAll(x => x.prefab is T);
            foreach (var list in lists)
            {
                foreach (var item in list.behaviours)
                {
                    if (item.gameObject.activeInHierarchy)
                    {
                        results.Add(item as T);
                    }                   
                }                
            }
            return results;
        }

        public List<MonoBehaviour> GetAllActiveItems()
        {
            List<MonoBehaviour> results = new List<MonoBehaviour>();
            foreach (var list in typeLists)
            {
                results.AddRange(list.behaviours);
            }
            return results;
        }

        public void DeactiveAll()
        {
            if (!Application.isPlaying)
            {
                DestroyAll();
                return;
            }
            for (int i = typeLists.Count - 1; i >= 0; i--)
            {
                for (int j = typeLists[i].behaviours.Count - 1; j >= 0; j--)
                {
                    typeLists[i].behaviours[j].gameObject.SetActive(false);
                }
            }
        }
        public void DestroyAll()
        {
            for (int i = typeLists.Count - 1; i >= 0; i--)
            {
                for (int j = typeLists[i].behaviours.Count - 1; j >= 0; j--)
                {
                    UnityEngine.Object.Destroy(typeLists[i].behaviours[j].gameObject);
                }
            }
        }

        private T FindDeactivatedObject<T>(MonoBehaviour prefab) where T : MonoBehaviour
        {
            TypeList typeList = GetTypeList(prefab);
            if (typeList == null)
            {
                return null;
            }
            T behaviour = typeList.behaviours.Find(x => !x.gameObject.activeInHierarchy) as T;
            return behaviour;
        }

        private T CreateCopy<T>(T prefab) where T : MonoBehaviour
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
            typeList.behaviours.Add(clone);
            return clone;
        }

        public T Spawn<T>(T prefab) where T : MonoBehaviour
        {
            if (prefab == null)
            {
                return null;
            }
            T mono = FindDeactivatedObject<T>(prefab);
            if (mono == null)
            {
                mono = CreateCopy(prefab);
            }
            mono.gameObject.SetActive(true);
            return mono;
        }

        public class TypeList
        {
            public MonoBehaviour prefab;
            public List<MonoBehaviour> behaviours;

            public TypeList(MonoBehaviour prefab)
            {
                this.prefab = prefab;
                behaviours = new List<MonoBehaviour>();
            }
        }
    }
}