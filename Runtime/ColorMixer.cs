using System.Collections;
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
            float r = 0;
            float g = 0;
            float b = 0;
            float a = 0;

            float totalStrength = 0;

            foreach (var colorValue in colorValues)
            {
                r += colorValue.color.r * colorValue.strength;
                g += colorValue.color.g * colorValue.strength;
                b += colorValue.color.b * colorValue.strength;
                a += colorValue.color.a * colorValue.strength;

                totalStrength += colorValue.strength;
            }

            return new Color(r / totalStrength, g / totalStrength, b / totalStrength, a / totalStrength);
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
            foreach (var colorValue in colorValues)
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