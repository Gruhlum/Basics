using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class SpawnableSpawnController<T> : BaseSpawnController<T, SpawnableSpawner<T>> where T : Component, ISpawnable<T>
    {
        protected override SpawnableSpawner<T> Spawner
        {
            get
            {
                return spawner;
            }
        }
        [SerializeField] private SpawnableSpawner<T> spawner;
    }
}