using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames
{
    public enum SizeType { Canvas, Custom }
    public enum OrthographicType { Camera, Custom }

    [System.Serializable]
    public class GridSettings
    {
        [Header("Grid Size")]

        [SerializeField] private SizeType sizeType = default;
        [SerializeField][DrawIf(nameof(sizeType), SizeType.Canvas)] private Canvas canvas = default;
        //[SerializeField][DrawIf(nameof(sizeType), SizeType.Custom)] private Vector2 gridSize = new Vector2(1920, 1080);

        [SerializeField][DrawIf(nameof(sizeType), SizeType.Canvas)] private OrthographicType orthographicType = default;
        [SerializeField][DrawIf(nameof(orthographicType), OrthographicType.Camera)] private Camera camera = default;
        [SerializeField][DrawIf(nameof(orthographicType), OrthographicType.Custom)] private float targetOrthographicSize = 5;
        [Space]
        [SerializeField] private Vector2 center = default;
        [Space]
        public Vector2 cellSize;
        [Space]
        public int radius = 3;

        public Canvas Canvas
        {
            get
            {
                return canvas;
            }
        }

        //public Vector2 GridSize
        //{
        //    get
        //    {
        //        if (SizeType == SizeType.Canvas)
        //        {
        //            return new Vector2(canvas.pixelRect.width, canvas.pixelRect.height);
        //        }
        //        else return gridSize;
        //    }
        //}

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

        public Vector2 Center
        {
            get
            {
                return this.center;
            }
            set
            {
                this.center = value;
            }
        }

        public GridSettings()
        {
        }
        public GridSettings(Vector2 gridSize, Vector2 cellSize, int radius) : this(cellSize, radius)
        {
            this.SizeType = SizeType.Custom;
            //this.gridSize = gridSize;
        }
        public GridSettings(Vector2 cellSize, int radius)
        {
            this.cellSize = cellSize;
            this.radius = radius;
        }

    }
}