using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    [System.Serializable]
    public class MultiGrid<T> where T : ISpawnable<T>
    {
        [SerializeField] private GridSettings gridSettings = default;
        [SerializeField] private GridType gridType = default;
        private List<Grid<T>> grids = new List<Grid<T>>();


        public MultiGrid(GridSettings gridSettings)
        {
            this.gridSettings = gridSettings;
            InstantiateGrid(gridSettings);

        }

        private Grid<T> InstantiateGrid(GridSettings gridSettings)
        {
            if (gridType == GridType.Rect)
            {
                var grid = new RectGrid<T>(gridSettings);
                grids.Add(grid);
                return grid;
            }
            else return null;
        }

        public List<Cell<T>> GetCells()
        {
            return grids[0].GetCells();
        }

        public Vector2 GetPosition(T obj)
        {
            for (int i = 0; i < grids.Count; i++)
            {
                var result = grids[i].GetPosition(obj);
                if (result.HasValue)
                {
                    return result.Value;
                }
            }
            Debug.Log("Adding Grid!");
            Grid<T> grid = InstantiateGrid(gridSettings);
            var final = grid.GetPosition(obj);
            if (final.HasValue)
            {
                return final.Value;

            }
            Debug.Log("SHOULD NOT HAPPEN");

            return Vector2.zero;
        }
    }
}