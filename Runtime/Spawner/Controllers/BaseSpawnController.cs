using System;
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
            Spawner.RemoveEmptyElements();
            if (OnObjectRequested != null && OnObjectRequested.GetInvocationList().Length > 0)
            {
                Debug.LogWarning("Event already has a listener. Probably multiple SpawnControllers in the scene!");
                return;
            }
            OnObjectRequested += SpawnController_OnObjectRequested;
        }

        private void OnDestroy()
        {
            Spawner.DestroyAll();
        }

        private T SpawnController_OnObjectRequested()
        {
            return Spawner.Spawn();
        }

        public static T Spawn()
        {
            return OnObjectRequested?.Invoke();
        }
    }
}