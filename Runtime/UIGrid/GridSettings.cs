using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames
{
    public enum SizeType { Screen, Custom }

    [System.Serializable]
    public class GridSettings
    {
        [Header("Grid Size")]
        [SerializeField] private SizeType sizeType = default;
        [SerializeField][DrawIf(nameof(sizeType), SizeType.Custom)] private int width = 1920;
        [SerializeField][DrawIf(nameof(sizeType), SizeType.Custom)] private int height = 1080;

        [Header("Cell")]
        public int cellWidth = 160;
        public int cellHeight = 40;
        [Space]
        public int maxRange = 3;

        public int Width
        {
            get
            {
                if (sizeType == SizeType.Screen)
                {
                    return Screen.width;
                }
                else return width;
            }
        }
        public int Height
        {
            get
            {
                if (sizeType == SizeType.Screen)
                {
                    return Screen.height;
                }
                else return height;
            }
        }


        public GridSettings()
        {
        }
        public GridSettings(int width, int height, int cellWidth, int cellHeight, int maxRange) : this(cellWidth, cellHeight, maxRange)
        {
            this.sizeType = SizeType.Custom;
            this.width = width;
            this.height = height;
        }
        public GridSettings(int cellWidth, int cellHeight, int maxRange)
        {
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.maxRange = maxRange;
        }

    }
}