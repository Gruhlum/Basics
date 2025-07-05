using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Performant spawner that requires objects to inherit ISpawnable.
    /// Objects notify spawner when they are disabled, so we don't need to find a disabled object first when we need a new one.
    /// </summary>
    [System.Serializable]
    public class SpawnableSpawner<T> : PoolSpawner<T> where T : Component, ISpawnable<T>
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

        public override List<T> Spawn(int amount, bool activate = true)
        {
            List<T> results = new List<T>(amount);

            for (int i = 0; i < amount; i++)
            {
                results.Add(Spawn(activate));
            }
            return results;
        }
        public override T Spawn(bool activate = true)
        {
            T spawnable = base.Spawn(activate);
            spawnable.OnDeactivated += Spawnable_OnDeactivated;
            return spawnable;
        }
        public override void DeactivateAll()
        {
            List<T> activeItems = activeInstances.ToList();

            for (int i = activeItems.Count - 1; i >= 0; i--)
            {
                activeItems[i].OnDeactivated -= Spawnable_OnDeactivated;
                activeItems[i].gameObject.SetActive(false);
                deactivatedInstances.Push(activeItems[i]);
            }
            activeInstances.Clear();
        }

        public override IEnumerable<T> GetActiveInstances()
        {
            return activeInstances;
        }

        protected override T GetEmptyInstance()
        {
            if (deactivatedInstances.Count > 0)
            {
                return deactivatedInstances.Pop();
            }
            else return CreateNewInstance();
        }
        private void Spawnable_OnDeactivated(T spawnable)
        {
            deactivatedInstances.Push(spawnable);
            spawnable.OnDeactivated -= Spawnable_OnDeactivated;
            Remove(spawnable);
        }
    }
}