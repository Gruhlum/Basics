using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames
{

    [System.Serializable]
    public class GridSettings
    {
        [SerializeField] private Vector2 center = default;
        [Space]
        public Vector2 cellSize;
        [Space]
        public int radius = 3;

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
        public GridSettings(Vector2 cellSize, int radius)
        {
            this.cellSize = cellSize;
            this.radius = radius;
        }

    }
}