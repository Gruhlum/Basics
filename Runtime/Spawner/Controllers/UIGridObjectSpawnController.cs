using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics.UIGrid;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class UIGridObjectSpawnController<T> : SpawnableSpawnController<T> where T : Component, ISpawnable<T>
    {
        public static Grid<T> uiGrid;

        protected abstract int GridWidth
        {
            get;
        }
        protected abstract int GridHeight
        {
            get;
        }
        protected abstract int CellWidth
        {
            get;
        }
        protected abstract int CellHeight
        {
            get;
        }
        protected override void Awake()
        {
            base.Awake();
            uiGrid = new Grid<T>(GridWidth, GridHeight, CellWidth, CellHeight);
        }

        public static Vector2 GetGridPosition(Vector2 viewportPosition, T spawnable)
        {
            Debug.Log(uiGrid == null);
            return uiGrid.GetPosition(viewportPosition, spawnable);
        }
    }
}