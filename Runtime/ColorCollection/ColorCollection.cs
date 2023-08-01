using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    public class ColorCollection
    {
        public static Color GetHealthColor(float percent)
        {
            //int total = 300;
            //int start = 80;
            //int max = 220;
            int current = Mathf.RoundToInt(300f - 300f * percent);

            int green = Mathf.Min((80 + current), 220);
            int red = 220 - (80 - (220 - current));
            return new Color(green / 255f, red / 255f, 60f / 225f);
        }
        public static Color InverseColor(Color col)
        {
            col.r = 1f - col.r;
            col.g = 1f - col.g;
            col.b = 1f - col.b;
            return col;
        }
    }
}