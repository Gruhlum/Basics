using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class SpawnController<T> : BaseSpawnController<T, Spawner<T>> where T : Component
    {
        protected override Spawner<T> Spawner
        {
            get
            {
                return spawner;
            }
        }
        [SerializeField] private Spawner<T> spawner = default;
    }
}