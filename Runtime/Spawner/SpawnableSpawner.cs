using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Performant spawner that requires objects to inherit ISpawnable.
    /// Objects notify spawner when they are disabled, so we don't need to find a disabled object first when we need a new one.
    /// </summary>
    [System.Serializable]
    public class SpawnableSpawner<T> : PoolSpawner<T> where T : Component, ISpawnable
    {
        protected override HashSet<T> Instances
        {
            get
            {
                return activeInstances;
            }
        }
        private HashSet<T> activeInstances = new HashSet<T>();
        private Stack<T> deactivatedInstances = new Stack<T>();

        public override T Spawn(bool activate)
        {
            ISpawnable spawnable = base.Spawn(activate);
            spawnable.OnDeactivated += Spawnable_OnDeactivated;
            return spawnable as T;
        }
        protected override T GetEmptyInstance()
        {
            if (deactivatedInstances.Count > 0)
            {
                return deactivatedInstances.Pop();
            }
            else return CreateNewInstance();
        }
        private void Spawnable_OnDeactivated(ISpawnable obj)
        {
            deactivatedInstances.Push(obj as T);
            Remove(obj as T);
        }
    }
}