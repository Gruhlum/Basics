using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public class ColorStack : BaseStack<Color>
    {
        public ColorStack(int layers) : base(layers)
        {
        }

        public ColorStack(int layers, float rotationTime) : base(layers, rotationTime)
        {
        }

        //public bool LerpColors
        //{
        //    get
        //    {
        //        return this.lerpColors;
        //    }
        //    set
        //    {
        //        this.lerpColors = value;
        //    }
        //}
        //private bool lerpColors;



        //private Color SolveMultiColor(List<Color> colors)
        //{
        //    if (colors.Count == 1)
        //    {
        //        return colors[0];
        //    }
        //    if (RotationDuration <= 0)
        //    {
        //        return MergeColors(colors);
        //    }
        //    else if (LerpColors)
        //    {
        //        return GetLerpColors(colors);
        //    }
        //    else
        //    {
        //        if (colors.Count <= rotationIndex)
        //        {
        //            rotationIndex = 0;
        //        }
        //        if (colors.Count <= rotationIndex)
        //        {
        //            return Color.white;
        //        }
        //        else return colors[rotationIndex];
        //    }
        //}
        //private Color GetLerpColors(List<Color> colors)
        //{
        //    if (colors.Count == 0)
        //    {
        //        return Color.white;
        //    }
        //    if (colors.Count == 1)
        //    {
        //        return colors[0];
        //    }

        //    Color col1 = colors[rotationIndex];
        //    Color col2;
        //    if (colors.Count <= rotationIndex + 1)
        //    {
        //        col2 = colors[0];
        //    }
        //    else col2 = colors[rotationIndex + 1];
        //    return Color.Lerp(col1, col2, rotationTimer / rotationDuration);
        //}
        //private Color MergeColors(List<Color> colors)
        //{
        //    float r = 0;
        //    float g = 0;
        //    float b = 0;

        //    foreach (var color in colors)
        //    {
        //        r += color.r;
        //        g += color.g;
        //        b += color.b;
        //    }

        //    return new Color(r / colors.Count, g / colors.Count, b / colors.Count);
        //}
        protected override bool CompareItems(Color item1, Color item2)
        {
            return item1.Compare(item2);
        }
    }
}