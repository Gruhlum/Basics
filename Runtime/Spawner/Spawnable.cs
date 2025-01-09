using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class Spawnable : MonoBehaviour, ISpawnable
    {
        public event Action<ISpawnable> OnDeactivated;

        protected virtual void OnDisable()
        {
            OnDeactivated?.Invoke(this);
        }
    }
}