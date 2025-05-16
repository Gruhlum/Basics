using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    [System.Serializable]
    public class Grid<T> where T : ISpawnable<T>
    {
        public Cell<T>[,] Cells;

        public int cellWidth;
        public int cellHeight;
        public int rows;
        public int columns;
        public int totalWidth;
        public int totalHeight;
        public int maxRange = 3;

        public Vector2 offset;

        private float scaleMultiplier = 1f;

        public float ScaleMultiplier
        {
            get
            {
                return this.scaleMultiplier;
            }
            private set
            {
                this.scaleMultiplier = value;
            }
        }

        public Grid(GridSettings gridSettings)
        {
            if (gridSettings.SizeType == SizeType.Canvas)
            {
                ScaleMultiplier = CalculateScaleMultiplier(gridSettings.Canvas, Camera.main);
            }
            GenerateGrid(gridSettings.Width, gridSettings.Height, gridSettings.cellWidth, gridSettings.cellHeight);
        }
        public Grid(int width, int height, int cellWidth, int cellHeight)
        {
            GenerateGrid(width, height, cellWidth, cellHeight);
        }
        private void GenerateGrid(int width, int height, int cellWidth, int cellHeight)
        {
            Debug.Log(width + " - " + height + " - " + cellWidth + " - " + cellHeight);
            rows = Mathf.FloorToInt(width / cellWidth * ScaleMultiplier);
            if (rows % 2 == 0)
            {
                rows--;
            }
            columns = Mathf.FloorToInt(height / cellHeight * ScaleMultiplier);

            Debug.Log("Rows: " + rows + " - " + columns);

            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.totalWidth = width;
            this.totalHeight = height;

            offset = CalculateOffsetValue();

            Cells = new Cell<T>[rows, columns];

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    Cells[x, y] = new Cell<T>(this, x, y, cellWidth, cellHeight);
                }
            }
        }
        private float CalculateScaleMultiplier(Canvas canvas, Camera camera)
        {
            // Canvas Scaling:
            // CameraSize * 2 / CanvasHeight
            if (canvas.renderMode != RenderMode.WorldSpace)
            {
                return 1f;
            }

            float targetScaling = camera.orthographicSize * 2f / canvas.pixelRect.height;

            Debug.Log(targetScaling + " - " + canvas.transform.localScale.x + " - " + (targetScaling / canvas.transform.localScale.x));

            return  targetScaling / canvas.transform.localScale.x;
        }

        private Vector2 CalculateOffsetValue()
        {
            //float x = cellWidth / 2f / totalCellsX + offsetX / 2f / totalCellsX;
            //float y = cellHeight / 2f / totalCellsY + offsetY / 2f / totalCellsY;

            //Debug.Log("leftover: " + (totalWidth - cellWidth * rows) + " - " + (totalHeight - cellHeight * columns));

            Vector2 cellSizeOffset = new Vector2(cellWidth / 2f / totalWidth, cellHeight / 2f / totalHeight);

            float leftOverX = (totalWidth - cellWidth * rows) / rows / 2f / totalWidth;
            float leftOverY = (totalHeight - cellHeight * columns) / columns / 2f / totalHeight;
            Vector2 gapOffset = new Vector2(leftOverX, leftOverY);
            //Debug.Log(cellSizeOffset + " - " + leftOverOffset);
            return cellSizeOffset + gapOffset;
        }
        public Vector2 GetPosition(Vector2 viewportPosition, T obj)
        {
            var cell = GetClosestCell(viewportPosition, false);
            cell.SetObject(obj);

            return cell.CalculateViewportPosition();
        }

        private List<Cell<T>> GetNeighbours(List<Cell<T>> cellsToCheck)
        {
            List<Cell<T>> results = new List<Cell<T>>();

            foreach (var cell in cellsToCheck)
            {
                results.AddRange(GetNeighbours(cell));
            }

            return results;
        }
        private List<Cell<T>> GetNeighbours(Cell<T> center)
        {
            List<Cell<T>> results = new List<Cell<T>>();

            Vector2Int direction = Vector2Int.up;

            for (int i = 0; i < 4; i++)
            {
                int targetX = center.x + direction.x;

                if (targetX < 0 || targetX >= rows)
                {
                    continue;
                }
                int targetY = center.y + direction.y;
                if (targetY < 0 || targetY >= columns)
                {
                    continue;
                }
                results.Add(Cells[targetX, targetY]);
                direction = RotateDirection(direction, true);
            }

            return results;
        }
        public Cell<T> GetClosestCell(Vector2 viewportPosition, bool emptyCell = true)
        {
            float positionX = Mathf.Lerp(0, rows - 1, viewportPosition.x);
            float positionY = Mathf.Lerp(0, columns - 1, viewportPosition.y);

            return GetClosestCell(positionX, positionY, emptyCell);
        }
        public Cell<T> GetClosestCell(float positionX, float positionY, bool emptyCell = true)
        {
            Cell<T> cell = GetCenterCell(positionX, positionY);
            if (!emptyCell || cell.spawnable == null)
            {
                return cell;
            }
            List<List<Cell<T>>> cellsToCheck = new List<List<Cell<T>>>();

            var directNeighbours = GetNeighbours(cell);
            foreach (var neigbour in directNeighbours)
            {
                if (neigbour.spawnable == null)
                {
                    return neigbour;
                }
                else
                {
                    var result = new List<Cell<T>>() { neigbour };
                    cellsToCheck.Add(result);
                }
            }

            HashSet<Cell<T>> checkedCells = new HashSet<Cell<T>>() { cell };

            int currentRange = 1;

            while (currentRange < maxRange && cellsToCheck.Any(x => x.Count > 0))
            {
                currentRange++;
                for (int i = 0; i < 4; i++)
                {
                    if (cellsToCheck[i].Count == 0)
                    {
                        continue;
                    }
                    List<Cell<T>> neighbours = GetNeighbours(cellsToCheck[i]);
                    cellsToCheck[i].Clear();
                    foreach (var neighbour in neighbours)
                    {
                        if (checkedCells.Contains(neighbour))
                        {
                            continue;
                        }
                        if (neighbour.spawnable == null)
                        {
                            return neighbour;
                        }
                        checkedCells.Add(neighbour);
                        cellsToCheck[i].Add(neighbour);
                    }
                }
            }
            return null;
        }

        private Cell<T> GetCenterCell(float positionX, float positionY)
        {
            int roundedX = Mathf.RoundToInt(positionX);
            int roundedY = Mathf.RoundToInt(positionY);

            var cell = Cells[roundedX, roundedY];
            return cell;
        }


        private Vector2Int RotateDirection(Vector2Int direction, bool clockwise)
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

        public List<Cell<T>> GetCells()
        {
            List<Cell<T>> results = new List<Cell<T>>();
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    results.Add(Cells[x, y]);
                }
            }
            return results;
        }
    }
}