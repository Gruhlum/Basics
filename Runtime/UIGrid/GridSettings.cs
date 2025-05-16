using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames
{
    public enum SizeType { Canvas, Custom }

    [System.Serializable]
    public class GridSettings
    {
        [Header("Grid Size")]
        [SerializeField] private SizeType sizeType = default;
        [SerializeField][DrawIf(nameof(SizeType), SizeType.Canvas)] private Canvas canvas = default;
        [SerializeField][DrawIf(nameof(SizeType), SizeType.Custom)] private int width = 1920;
        [SerializeField][DrawIf(nameof(SizeType), SizeType.Custom)] private int height = 1080;

        [Header("Cell")]
        public int cellWidth = 160;
        public int cellHeight = 40;
        [Space]
        public int maxRange = 3;

        public Canvas Canvas
        {
            get
            {
                return canvas;
            }
        }
        public int Width
        {
            get
            {
                if (SizeType == SizeType.Canvas)
                {
                    return (int)canvas.pixelRect.width;
                }
                else return width;
            }
        }
        public int Height
        {
            get
            {
                if (SizeType == SizeType.Canvas)
                {
                    return (int)canvas.pixelRect.height;
                }
                else return height;
            }
        }

        public SizeType SizeType
        {
            get
            {
                return this.sizeType;
            }

            set
            {
                this.sizeType = value;
            }
        }

        public GridSettings()
        {
        }
        public GridSettings(int width, int height, int cellWidth, int cellHeight, int maxRange) : this(cellWidth, cellHeight, maxRange)
        {
            this.SizeType = SizeType.Custom;
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