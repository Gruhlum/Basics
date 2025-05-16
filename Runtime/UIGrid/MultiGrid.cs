using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    [System.Serializable]
    public class MultiGrid<T> where T : ISpawnable<T>
    {
        [SerializeField] private GridSettings gridSettings = default;

        private List<Grid<T>> grids = new List<Grid<T>>();


        public MultiGrid(GridSettings gridSettings)
        {
            this.gridSettings = gridSettings;
            grids.Add(new Grid<T>(gridSettings));
        }

        public List<Cell<T>> GetCells()
        {
            return grids[0].GetCells();
        }

        public Vector2 GetPosition(Vector2 viewportPosition, T obj)
        {
            for (int i = 0; i < grids.Count; i++)
            {
                Cell<T> cell = grids[i].GetClosestCell(viewportPosition);
                if (cell != null)
                {
                    cell.SetObject(obj);
                    //Debug.Log(cell);
                    return cell.CalculateViewportPosition();
                }
            }
            Debug.Log("Adding new grid: " + grids.Count);
            Grid<T> grid = new Grid<T>(gridSettings);
            grids.Add(grid);
            return grid.GetPosition(viewportPosition, obj);
        }
    }
}