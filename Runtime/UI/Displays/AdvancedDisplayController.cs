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

        [SerializeField] private GameObject dummyGO = default;
        [SerializeField] private bool hideDummyOnLimitReached = default;

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
                foreach (var item in items)
                {
                    SetupDisplay(item, SpawnDisplay());
                }
                if (dummyGO != null)
                {
                    dummyGO.transform.SetSiblingIndex(displaySpawner.TotalActiveInstances());
                }
                if (displaySpawner.TotalActiveInstances() >= maximumDisplays)
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
                if (displaySpawner.TotalActiveInstances() < calculateMinItems)
                {
                    for (int i = displaySpawner.TotalActiveInstances(); i < calculateMinItems; i++)
                    {
                        SetupDisplay(null, SpawnDisplay());
                    }
                }
            }
        }
    }
}