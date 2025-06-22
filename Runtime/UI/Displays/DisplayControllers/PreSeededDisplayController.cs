using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class PreSeededDisplayController<D, T> : DisplayControllerBase<D, T> where D : Display<D, T>
    {
        [SerializeField] protected List<T> items = default;
        [SerializeField] protected List<D> displays = default;
        [Space]
        [SerializeField] private BasicSpawner<D> displaySpawner = default;
        [Space]
        [SerializeField] private bool autoBuildDisplays = default;

        protected virtual void Reset()
        {
            displays = GetComponentsInChildren<D>().ToList();
        }

        protected virtual void OnValidate()
        {
            if (!autoBuildDisplays)
            {
                return;
            }
            if (gameObject.scene.name == null)
            {
                return;
            }
            
            if (RequiresRebuild())
            {
                EditorApplication.delayCall += () =>
                {
                    GenerateDisplays();
                };
            }
        }
        protected virtual void Awake()
        {
            FindDisplays();

            if (RequiresRebuild())
            {
                GenerateDisplays();
            }

            foreach (var display in displays)
            {
                AddDisplayEvents(display);
            }
        }

        private void FindDisplays()
        {
            displays = displaySpawner.Parent.GetComponentsInChildren<D>().ToList();
        }

        private bool RequiresRebuild()
        {
            if (items == null || items.Count == 0)
            {
                return false;
            }
            if (items.Count != displays.Count)
            {
                return true;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    continue;
                }
                if (!displays[i].Item.Equals(items[i]))
                {
                    return true;
                }
            }
            return false;
        }

        private void GenerateDisplays()
        {
            if (Application.isPlaying)
            {
                return;
            }

            displaySpawner.DestroyAll();
            displays = new List<D>();


            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }
                D display = PrefabUtility.InstantiatePrefab(displaySpawner.Prefab, displaySpawner.Parent) as D;
                display.SetItem(item);
                displays.Add(display);
            }
        }

        public void SelectFirstDisplay()
        {
            if (displays == null || displays.Count <= 0)
            {
                return;
            }
            displays[0].DisplayClicked();
        }

        public List<D> GetDisplays()
        {
            if (displays != null && displays.Count > 0)
            {
                return displays;
            }
            return null;
        }
    }
}