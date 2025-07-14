using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class ColorMixer
    {
        private List<ColorValue> colorValues = new List<ColorValue>();


        public void Add(Color color)
        {
            Add(color, 1);
        }
        public void Add(Color color, float strength)
        {
            colorValues.Add(new ColorValue(color, strength));
        }

        public Color Mix()
        {
            Color result = Color.clear;
            float totalStrength = 0;

            foreach (ColorValue colorValue in colorValues)
            {
                result += colorValue.color * colorValue.strength;
                totalStrength += colorValue.strength;
            }

            return result / totalStrength;
        }
        public void Reset()
        {
            colorValues.Clear();
        }

        public override string ToString()
        {
            if (colorValues == null || colorValues.Count == 0)
            {
                return "Empty Mixer";
            }

            List<string> colorTexts = new List<string>();
            foreach (ColorValue colorValue in colorValues)
            {
                colorTexts.Add(colorValue.ToString());
            }
            return $"Mixer ({colorValues.Count}) {string.Join(" | ", colorTexts)}";
        }

        private struct ColorValue
        {
            public Color color;
            public float strength;

            public ColorValue(Color color, float strength = 1)
            {
                this.color = color;
                this.strength = strength;
            }

            public override string ToString()
            {
                return $"{color} {strength}";
            }
        }
    }
}