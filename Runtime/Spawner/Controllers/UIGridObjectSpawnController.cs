using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics.UIGrid;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class UIGridObjectSpawnController<T> : SpawnableSpawnController<T> where T : Component, ISpawnable<T>
    {
        public static MultiGrid<T> uiGrid;


        private static Vector3 canvasScaling = Vector3.one;
        [SerializeField] private Canvas parentCanvas = default;

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
        [SerializeField] private bool showGridCells = default;
        [SerializeField, DrawIf(nameof(showGridCells), true)] private CellDisplayController cellDisplayController = default;

        protected override void Awake()
        {
            base.Awake();
            uiGrid = new MultiGrid<T>(GridSettings);
            canvasScaling = parentCanvas.transform.localScale;
            
            if (showGridCells && cellDisplayController != null)
            {
                cellDisplayController.DisplayCells(uiGrid.GetCells());
            }
        }

        public static Vector3 GetGridPosition(Vector3 position, T spawnable)
        {
            //Debug.Log(canvasScaling);
            return uiGrid.GetPosition(position, spawnable) * canvasScaling;
        }
    }
}