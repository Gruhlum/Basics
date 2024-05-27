using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        protected override List<T> Instances
        {
            get
            {
                return items;
            }
        }
        [HideInInspector] protected static readonly List<T> items = new List<T>();

        private bool isInit = false;

        private void Init()
        {
            RemoveEmptyElements();
            isInit = true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override T Spawn(bool activate)
        {
            if (isInit == false)
            {
                Init();
            }
            return base.Spawn(activate);
        }
    }
}