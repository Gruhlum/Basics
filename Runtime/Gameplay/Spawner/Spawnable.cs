using System;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class Spawnable : MonoBehaviour, ISpawnable<Spawnable>
    {
        public event Action<Spawnable> OnDeactivated;

        protected virtual void OnDisable()
        {
            OnDeactivated?.Invoke(this);
        }
    }
}