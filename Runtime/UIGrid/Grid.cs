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

        public Vector2 offset;

        public Grid(int width, int height, int cellWidth, int cellHeight)
        {
            Debug.Log(width + " - " + height + " - " + cellWidth + " - " + cellHeight);
            rows = width / cellWidth;
            if (rows % 2 == 0)
            {
                rows--;
            }
            columns = height / cellHeight;
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
            float positionX = (Mathf.Lerp(0, rows - 1, viewportPosition.x));
            float positionY = (Mathf.Lerp(0, columns - 1, viewportPosition.y));

            //Debug.Log(positionX + " - " + positionY);
            //List<Cell> cells = GetRequiredCells(positionX, positionY, width, height);

            //foreach (var cell in cells)
            //{
            //    cell.occupyingObject = obj;
            //}

            //float targetX = cells.Sum(cell => cell.x) / cells.Count;
            //float targetY = cells.Sum(cell => cell.y) / cells.Count;
            var cell = GetClosestCell(positionX, positionY, 3);
            if (cell == null)
            {
                cell = Cells[Mathf.RoundToInt(positionY), Mathf.RoundToInt(positionY)];
                return cell.CalculateViewportPosition();
            }
            else cell.SetObject(obj);

            return cell.CalculateViewportPosition();
        }

        private Cell<T> GetClosestCell(float positionX, float positionY, int maxRange)
        {

            int roundedX = Mathf.RoundToInt(positionX);
            int roundedY = Mathf.RoundToInt(positionY);

            var cell = Cells[roundedX, roundedY];

            if (cell.spawnable == null)
            {
                return cell;
            }

            int distanceX = 1;
            int distanceY = 1;

            for (int range = 0; range < maxRange; range++)
            {
                Vector2Int direction = Vector2Int.up;

                for (int i = 0; i < 4; i++)
                {
                    Cell<T> target = Cells[roundedX + direction.x * (distanceX + range), roundedY + direction.y * (distanceY + range)];
                    if (target.spawnable == null)
                    {
                        return target;
                    }
                    direction = RotateDirection(direction, true);
                }

                direction = new Vector2Int(1, 1);

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < range; j++)
                    {
                        Cell<T> target = Cells[roundedX + direction.x * (distanceX + range), roundedY + direction.y * (distanceY + range)];
                        if (target.spawnable == null)
                        {
                            return target;
                        }
                        direction = RotateDirection(direction, true);
                    }
                   
                }
            }
            return cell;
        }

        //private Cell<T> GetClosestCell(float positionX, float positionY, int maxRange)
        //{
        //    // Spiral from target position until an empty cell is found
        //    // Don't ignore grid bounds
        //    // If no Cell is empty return position of target position

        //    int roundedX = Mathf.RoundToInt(positionX);
        //    int roundedY = Mathf.RoundToInt(positionY);

        //    var cell = Cells[roundedX, roundedY];
        //    var startCell = cell;
        //    if (cell.spawnable == null)
        //    {
        //        return cell;
        //    }
        //    Vector2Int startDirection;
        //    float differenceX = roundedX - positionX;
        //    float differenceY = roundedY - positionY;

        //    bool clockwise;

        //    if (Mathf.Abs(differenceX) < Mathf.Abs(differenceY))
        //    {
        //        if (differenceX > 0)
        //        {
        //            startDirection = Vector2Int.right;
        //        }
        //        else startDirection = Vector2Int.left;

        //        clockwise = differenceY > 0;
        //    }
        //    else
        //    {
        //        if (differenceY > 0)
        //        {
        //            startDirection = Vector2Int.up;
        //        }
        //        else startDirection = Vector2Int.down;

        //        clockwise = differenceX > 0;
        //    }

        //    for (int range = 0; range < maxRange; range++)
        //    {
        //        Vector2Int currentDirection = startDirection;
        //        // If we are on the last lap we have to make one extra turn to complete a full square
        //        int totalDirection = range + 1 == maxRange ? 5 : 4;
        //        Debug.Log(totalDirection);
        //        for (int direction = 0; direction < totalDirection; direction++)
        //        {
        //            int lineLength = 1 + (range * 4 + direction) / 2;
        //            if (direction + 1 == 5)
        //            {
        //                lineLength--;
        //                Debug.Log(lineLength);
        //            }
        //            int increaseX = currentDirection.x;
        //            int increaseY = currentDirection.y;

        //            for (int line = 0; line < lineLength; line++)
        //            {
        //                int targetX = cell.x + increaseX;
        //                int targetY = cell.y + increaseY;

        //                if (targetX < 0 || targetX >= rows)
        //                {
        //                    break;
        //                }
        //                if (targetY < 0 || targetY >= columns)
        //                {
        //                    break;
        //                }
        //                cell = Cells[targetX, targetY];
        //                if (cell.spawnable == null)
        //                {
        //                    return cell;
        //                }
        //            }
        //            currentDirection = RotateDirection(currentDirection, clockwise);
        //        }
        //    }
        //    Debug.Log("Could not find Cell!");
        //    return startCell;
        //}
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

        //private List<Cell<T>> GetRequiredCells(int centerX, int centerY, int width, int height)
        //{
        //    List<Cell<T>> results = new List<Cell<T>>();
        //    int requiredCellsX = Mathf.CeilToInt(width / (float)cellWidth);

        //    // We want the object to be centered on the X axis
        //    if (this.rows % 2 != 0 && requiredCellsX % 2 == 0 || this.rows % 2 == 0 && requiredCellsX % 2 != 0)
        //    {
        //        requiredCellsX++;
        //    }
        //    int requiredCellsY = Mathf.CeilToInt(height / (float)cellHeight);

        //    int startX = requiredCellsX / 2;
        //    int endX = requiredCellsX - startX;

        //    int startY = requiredCellsY / 2;
        //    int endY = requiredCellsY - startY;


        //    for (int x = startX; x < endX; x++)
        //    {
        //        for (int y = startY; y < endY; y++)
        //        {
        //            results.Add(Cells[x + centerX, y + centerY]);
        //        }
        //    }

        //    return results;
        //}
    }
}