using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    public abstract class DisplayController<T> : MonoBehaviour
    {
        [SerializeField] protected Spawner<Display<T>> displaySpawner = default;

        [SerializeField] private List<T> items = default;

        [ContextMenu("Generate Displays")]
        public void Setup()
        {
            displaySpawner.DeactivateAll();
            foreach (var obj in items)
            {
                displaySpawner.Spawn().Setup(obj, this);
            }
        }

        public abstract void OnDisplayClicked(Display<T> display);
    }
}