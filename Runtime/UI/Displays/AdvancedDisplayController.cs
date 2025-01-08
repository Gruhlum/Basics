using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
	public abstract class AdvancedDisplayController<T> : DisplayController<T> where T : class
    {
        [Header("Advanced")]
        [Min(0)][SerializeField] private int minimumDisplays = default;
        [Min(0)][SerializeField] private int maximumDisplays = default;

        [SerializeField] protected GameObject dummyGO = default;
        [SerializeField] private bool hideDummyOnLimitReached = default;

        public event Action OnDummyClicked;

        protected virtual void OnValidate()
        {
            if (maximumDisplays < minimumDisplays)
            {
                maximumDisplays = minimumDisplays;
            }
        }

        protected virtual void Awake()
        {
            if (minimumDisplays > 0)
            {
                DisplayItems();
            }
        }
        protected override void DisplayItems()
        {
            if (displays != null && displays.Count > 0)
            {
                for (int i = 0; i < displays.Count; i++)
                {
                    if (items.Count <= i)
                    {
                        SetupDisplay(null, displays[i]);
                    }
                    else SetupDisplay(items[i], displays[i]);
                }
            }
            else
            {
                displaySpawner.DeactivateAll();
                int totalActiveInstances = displaySpawner.TotalActiveInstances();
                foreach (var item in items)
                {
                    SetupDisplay(item, SpawnDisplay());
                }
                if (dummyGO != null)
                {
                    dummyGO.transform.SetSiblingIndex(totalActiveInstances);
                }
                if (totalActiveInstances >= maximumDisplays)
                {
                    if (hideDummyOnLimitReached)
                    {
                        dummyGO.SetActive(false);
                    }
                    return;
                }
                else dummyGO.SetActive(true);
                int calculateMinItems = minimumDisplays;
                if (dummyGO != null)
                {
                    calculateMinItems -= 1;
                }
                if (totalActiveInstances < calculateMinItems)
                {
                    for (int i = totalActiveInstances; i < calculateMinItems; i++)
                    {
                        SetupDisplay(null, SpawnDisplay());
                    }
                }
            }
        }

        public virtual void DummyClicked()
        {
            OnDummyClicked?.Invoke();
        }
    }
}