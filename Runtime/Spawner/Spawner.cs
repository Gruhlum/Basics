using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Spawns objects and holds references to them. Inactive objects will be reused before spawning new ones.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class Spawner<T> : PoolSpawner<T> where T : Component
    {
        protected override HashSet<T> Instances
        {
            get
            {
                return items;
            }
        }
        protected readonly HashSet<T> items = new HashSet<T>();
    }
}