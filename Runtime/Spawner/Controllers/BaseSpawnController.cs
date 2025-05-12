using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class BaseSpawnController<T, S> : MonoBehaviour where T : Component where S : PoolSpawner<T>
    {
        protected abstract S Spawner
        {
            get;
        }

        private static event Func<T> OnObjectRequested;

        protected virtual void Awake()
        {
            OnObjectRequested += SpawnController_OnObjectRequested;
        }

        private T SpawnController_OnObjectRequested()
        {
            return Spawner.Spawn();
        }

        public static T SpawnObject()
        {
            return OnObjectRequested?.Invoke();
        }
    }
}