using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    [System.Serializable]
    public abstract class Grid<T> where T : ISpawnable<T>
    {
        public Dictionary<Coord, Cell<T>> Cells = new Dictionary<Coord, Cell<T>>();

        //public Vector2 offset;
        protected Vector2 center;
        public int radius;

        public Grid(GridSettings gridSettings)
        {
            center = gridSettings.Center;
            radius = gridSettings.radius;
        }

        protected abstract void GenerateGrid(Vector2 cellSize, int maxRange);
        //protected void CalculateScaleMultiplier(Canvas canvas, Camera camera)
        //{
        //    // Canvas Scaling:
        //    // CameraSize * 2 / CanvasHeight
        //    if (canvas.renderMode != RenderMode.WorldSpace)
        //    {
        //        ScaleMultiplier = 1f;
        //    }
        //    float targetScaling = camera.orthographicSize * 2f / canvas.pixelRect.height;
        //    ScaleMultiplier = targetScaling / canvas.transform.localScale.x;
        //}

        public Vector2? GetPosition(Vector2 position, T obj)
        {
            Cell<T> cell = GetEmptyCell(position);
            if (cell != null)
            {
                cell.SetObject(obj);
                return cell.GetPosition() + center;
            }
            return null;
        }
        public abstract Cell<T> GetEmptyCell(Vector2 position);
        public List<Cell<T>> GetCells()
        {
            List<Cell<T>> results = new List<Cell<T>>();
            foreach (var cell in Cells.Values)
            {
                results.Add(cell);
            }
            return results;
        }
    }
}