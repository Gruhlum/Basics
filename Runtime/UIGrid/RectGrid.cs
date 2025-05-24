using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    [System.Serializable]
    public class RectGrid<T> : Grid<T> where T : ISpawnable<T>
    {
        public Vector2 cellSize;

        public RectGrid(GridSettings gridSettings) : base(gridSettings)
        {
            GenerateGrid(gridSettings.cellSize, gridSettings.radius);
        }

        protected override void GenerateGrid(Vector2 cellSize, int radius)
        {
            this.cellSize = cellSize;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) > radius)
                    {
                        continue;
                    }
                    Coord coord = new Coord(x, y);
                    Cells.Add(coord, new Cell<T>(coord, cellSize));
                }
            }
        }

        public override Cell<T> GetEmptyCell()
        {
            var coord = GetClosestEmptyCell();

            if (coord.HasValue && Cells.TryGetValue(coord.Value, out Cell<T> cell) && cell.spawnable == null)
            {
                return cell;
            }
            if (Cells.TryGetValue(Coord.zero, out Cell<T> result))
            {
                return result;
            }
            return null;
        }

        public Coord? GetClosestEmptyCell()
        {
            Coord coord = Coord.zero;
            if (IsEmpty(coord))
            {
                return coord;
            }
            List<List<Coord>> coordsToCheck = new List<List<Coord>>();

            var directNeighbours = GetNeighbours(coord);

            foreach (var neigbour in directNeighbours)
            {
                if (IsEmpty(neigbour))
                {
                    return neigbour;
                }
                else
                {
                    var newList = new List<Coord>() { neigbour };
                    coordsToCheck.Add(newList);
                }
            }

            HashSet<Coord> checkedCells = new HashSet<Coord>() { coord };

            while (coordsToCheck.Any(x => x.Count > 0))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (coordsToCheck[i].Count == 0)
                    {
                        continue;
                    }
                    List<Coord> neighbours = GetNeighbours(coordsToCheck[i]);
                    coordsToCheck[i].Clear();
                    foreach (var neighbour in neighbours)
                    {
                        if (checkedCells.Contains(neighbour))
                        {
                            continue;
                        }
                        if (IsEmpty(neighbour))
                        {
                            return neighbour;
                        }
                        checkedCells.Add(neighbour);
                        coordsToCheck[i].Add(neighbour);
                    }
                }
            }
            return null;
        }
        private bool IsEmpty(Coord coord)
        {
            if (Cells.TryGetValue(coord, out Cell<T> cell) && cell.spawnable == null)
            {
                return true;
            }
            return false;
        }
        protected List<Coord> GetNeighbours(List<Coord> cellsToCheck)
        {
            List<Coord> results = new List<Coord>();

            foreach (var cell in cellsToCheck)
            {
                results.AddRange(GetNeighbours(cell));
            }

            return results;
        }
        protected List<Coord> GetNeighbours(Coord centerCoord)
        {
            List<Coord> results = new List<Coord>();

            Coord direction = Coord.up;

            for (int i = 0; i < 4; i++)
            {
                Coord result = centerCoord + direction;

                if (!Cells.ContainsKey(result))
                {
                    continue;
                }
                results.Add(result);
                direction.Rotate(Coord.zero, 1);
            }
            return results;
        }

        protected Vector2Int RotateDirection(Vector2Int direction, bool clockwise)
        {
            if (clockwise)
            {
                return new Vector2Int(direction.y, -direction.x);
            }
            else
            {
                return new Vector2Int(-direction.y, direction.x);
            }
        }


    }
}