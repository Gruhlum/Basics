using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    [System.Serializable]
    public class Cell<T> where T : ISpawnable<T>
    {
        public Coord coord;

        public int X
        {
            get
            {
                return coord.x;
            }
        }
        public int Y
        {
            get
            {
                return coord.y;
            }
        }

        public T spawnable;

        public Vector2 Size
        {
            get
            {
                return this.size;
            }
            private set
            {
                this.size = value;
            }
        }
        private Vector2 size;

        public Cell(Coord coord, Vector2 size)
        {
            this.coord = coord;
            this.Size = size;
        }

        public Vector2 GetPosition()
        {
            //Debug.Log(ToString());
            return ( new Vector2(coord.x * Size.x, coord.y * Size.y));
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

        public override string ToString()
        {
            return $"Coord: {coord.x}, {coord.y} Pos: {new Vector2(coord.x * Size.x, coord.y * Size.y) * 0.04f}";
        }
    }
}