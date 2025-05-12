using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    [System.Serializable]
    public class Cell<T> where T : ISpawnable<T>
    {
        public Grid<T> grid;

        public int x;
        public int y;

        public int width;
        public int height;

        public T spawnable;

        public Cell(Grid<T> grid, int x, int y, int width, int height)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void SetObject(T obj)
        {
            this.spawnable = obj;
            obj.OnDeactivated += Obj_OnDeactivated;
        }

        private void Obj_OnDeactivated(T obj)
        {
            obj.OnDeactivated -= Obj_OnDeactivated;
            spawnable = default;
        }

        public Vector2 CalculateViewportPosition()
        {
            var result = new Vector2(x / (float)grid.rows, y / (float)grid.columns) + grid.offset;
            //Debug.Log(ToString() + " - " + result);
            return result;
        }

        public Vector2 GetSize()
        {
            return new Vector2(width, height);
        }

        public override string ToString()
        {
            return $"Pos: {x}, {y}";
        }
    }
}