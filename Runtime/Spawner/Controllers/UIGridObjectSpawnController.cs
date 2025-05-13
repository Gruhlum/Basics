using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics.UIGrid;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class UIGridObjectSpawnController<T> : SpawnableSpawnController<T> where T : Component, ISpawnable<T>
    {
        public static MultiGrid<T> uiGrid;

        public abstract GridSettings GridSettings
        {
            get;
        }


        protected override void Awake()
        {
            base.Awake();
            uiGrid = new MultiGrid<T>(GridSettings);
        }

        public static Vector2 GetGridPosition(Vector2 viewportPosition, T spawnable)
        {
            return uiGrid.GetPosition(viewportPosition, spawnable);
        }
    }
}