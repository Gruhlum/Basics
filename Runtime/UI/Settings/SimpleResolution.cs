using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.UI
{
    [System.Serializable]
    public struct SimpleResolution
    {
        public int width;
        public int height;


        public Resolution ToResolution()
        {
            Resolution resolution = new Resolution
            {
                width = width,
                height = height
            };
            return resolution;
        }
        public override string ToString()
        {
            return $"{width}x{height}";
        }
    }
}