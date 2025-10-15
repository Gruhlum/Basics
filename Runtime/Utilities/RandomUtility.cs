using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HexTecGames
{
    public static class RandomUtility
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

        public static string CovertToDisplayName(string input)
        {
            if (input.Length <= 1)
            {
                return input.ToUpper();
            }

            StringBuilder result = new StringBuilder();
            result.Append(char.ToUpper(input[0]));

            for (int i = 1; i < input.Length; i++)
            {
                char c = input[i];
                if (char.IsUpper(c))
                {
                    result.Append($" {c}");
                }
                else result.Append(c);
            }

            return result.ToString();
        }

        public static Color GenerateRandomColor()
        {
            return GenerateRandomColor(Random.Range(0f, 1f));
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

        public static T GetRandomEnum<T>() where T : Enum
        {
            Array results = Enum.GetValues(typeof(T));
            int rng = Random.Range(0, results.Length);
            return (T)results.GetValue(rng);
        }

        public static int GenerateRandomNumber(int length)
        {
            int min = Mathf.RoundToInt(Mathf.Pow(10, length - 1));
            int max = min * 10;
            return Random.Range(min, max);
        }

        public static string GenerateRandomWord(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append((char)('A' + Random.Range(0, 26)));
            }
            return stringBuilder.ToString();
        }

        public static bool Coinflip()
        {
            return Random.value < 0.5f;
        }
    }
}