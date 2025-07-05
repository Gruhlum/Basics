using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class StaticSpawner<T> : PoolSpawner<T> where T : Component
    {
        protected override HashSet<T> Instances
        {
            get
            {
                return items;
            }
        }
        [HideInInspector] protected static readonly HashSet<T> items = new HashSet<T>();

        private bool isInit = false;

        private void Init()
        {
            RemoveEmptyElements();
            isInit = true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override T Spawn(bool activate = true)
        {
            if (isInit == false)
            {
                Init();
            }
            return base.Spawn(activate);
        }
    }
}