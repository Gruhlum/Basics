using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HexTecGames
{
    public static class Utility
    {
        public static int RollTotalProccs(int value)
        {
            int proccs = value / 100;
            int remainder = value % 100;
            if (Random.Range(0, 100) < remainder)
            {
                proccs++;
            }
            return proccs;
        }
        public static string ToRomanNumber(int number)
        {
            StringBuilder result = new StringBuilder();
            int[] digitsValues = { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000 };
            string[] romanDigits = { "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M" };
            while (number > 0)
            {
                for (int i = digitsValues.Count() - 1; i >= 0; i--)
                    if (number / digitsValues[i] >= 1)
                    {
                        number -= digitsValues[i];
                        result.Append(romanDigits[i]);
                        break;
                    }
            }
            return result.ToString();
        }
        public static Color GenerateRandomColor(float intensity)
        {
            int rngColor = Random.Range(0, 3);
            float lowValue = Mathf.Lerp(0.5f, 0, intensity);
            float someValue = Mathf.Lerp(0.5f, 1, intensity);
            float highValueMin = Random.Range(someValue / 2f, someValue);

            float r = rngColor == 0 ? lowValue : Random.Range(highValueMin, someValue);
            float g = rngColor == 1 ? lowValue : Random.Range(highValueMin, someValue);
            float b = rngColor == 2 ? lowValue : Random.Range(highValueMin, someValue);

            return new Color(r, g, b, 1);
        }
    }
}