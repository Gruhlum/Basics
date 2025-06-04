using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class AdvancedDisplayController<D, T> : DisplayController<D, T> where T : class where D : Display<D, T>
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
        public override void DisplayItems()
        {
            CreateRequiredDisplays();
        }
        private void CreateRequiredDisplays()
        {
            displaySpawner.DeactivateAll();
            int activeItems = 0;

            if (items != null)
            {
                activeItems = items.Count;
                foreach (var item in items)
                {
                    SetupDisplay(SpawnDisplay(), item);
                }
            }

            int minItems = minimumDisplays;

            if (dummyGO != null)
            {
                minItems--;
                dummyGO.transform.SetSiblingIndex(activeItems);

                if (activeItems >= maximumDisplays)
                {
                    if (hideDummyOnLimitReached)
                    {
                        dummyGO.SetActive(false);
                    }
                    return;
                }
                else dummyGO.SetActive(true);
            }

            if (activeItems < minItems)
            {
                for (int i = activeItems; i < minItems; i++)
                {
                    SetupDisplay(SpawnDisplay(), null);
                }
            }
        }

        public virtual void DummyClicked()
        {
            OnDummyClicked?.Invoke();
        }
    }
}