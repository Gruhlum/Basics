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
            //Debug.Log(width + " - " + height + " - " + cellWidth + " - " + cellHeight);

            //Debug.Log("Rows: " + rows + " - " + columns);

            this.cellSize = cellSize;

            //offset = CalculateOffsetValue();
            //Vector2 offset = new Vector2(cellSize.x * rows, cellSize.y * columns) / 2f;



            //Vector2Int centerPos = new Vector2Int(Mathf.FloorToInt(rows / 2f), Mathf.FloorToInt(columns / 2f));
            //Debug.Log($"{nameof(centerPos)} {centerPos}");
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

        //protected Vector2 CalculateOffsetValue()
        //{
        //    //float x = cellWidth / 2f / totalCellsX + offsetX / 2f / totalCellsX;
        //    //float y = cellHeight / 2f / totalCellsY + offsetY / 2f / totalCellsY;

        //    //Debug.Log("leftover: " + (totalWidth - cellWidth * rows) + " - " + (totalHeight - cellHeight * columns));

        //    Vector2 cellSizeOffset = new Vector2(cellWidth / 2f / totalWidth, cellHeight / 2f / totalHeight);

        //    float leftOverX = (totalWidth - cellWidth * rows) / rows / 2f / totalWidth;
        //    float leftOverY = (totalHeight - cellHeight * columns) / columns / 2f / totalHeight;
        //    Vector2 gapOffset = new Vector2(leftOverX, leftOverY);
        //    //Debug.Log(cellSizeOffset + " - " + leftOverOffset);
        //    return cellSizeOffset + gapOffset;
        //}
        public override Cell<T> GetEmptyCell(Vector2 position)
        {
            var coord = GetClosestEmptyCell(position * cellSize);

            if (coord.HasValue && Cells.TryGetValue(coord.Value, out Cell<T> cell) && cell.spawnable == null)
            {
                return cell;
            }
            return null;
        }

        public Coord? GetClosestEmptyCell(Vector2 position)
        {
            Coord coord = new Coord(position);

            if (IsEmpty(coord))
            {
                return coord;
            }
            List<List<Coord>> coordsToCheck = new List<List<Coord>>();

            var directNeighbours = GetNeighbours(coord);

            foreach (var neigbour in directNeighbours)
            {
                if (Cells.TryGetValue(neigbour, out Cell<T> result) && result.spawnable == null)
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
                        if (Cells.TryGetValue(neighbour, out Cell<T> cell) && cell.spawnable == null)
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