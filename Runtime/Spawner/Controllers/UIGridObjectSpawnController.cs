using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics.UIGrid;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class UIGridObjectSpawnController<T> : SpawnableSpawnController<T> where T : Component, ISpawnable<T>
    {
        public static MultiGrid<T> uiGrid;

        public virtual GridSettings GridSettings
        {
            get
            {
                return gridSettings;
            }
            protected set
            {
                gridSettings = value;
            }
        }
        [SerializeField] private GridSettings gridSettings = default;

        [Header("Debug")]
        [SerializeField] private CellDisplayController cellDisplayController = default;

        protected override void Awake()
        {
            base.Awake();
            uiGrid = new MultiGrid<T>(GridSettings);
            if (cellDisplayController != null)
            {
                cellDisplayController.DisplayCells(uiGrid.GetCells());
            }
        }

        public static Vector3 GetGridPosition(Vector3 position, T spawnable)
        {
            Vector2 targetViewportPosition = Camera.main.WorldToViewportPoint(position);
            Vector3 cellViewportPosition = uiGrid.GetPosition(targetViewportPosition, spawnable);
            return Camera.main.ViewportToWorldPoint(cellViewportPosition);
        }
    }
}