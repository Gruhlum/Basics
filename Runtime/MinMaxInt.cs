using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class MinMaxInt
    {
        public int MinValue;
        public int MaxValue;
        public int MinLimit;
        public int MaxLimit;

        public MinMaxInt()
        {
            MaxLimit = 10;
        }
        public MinMaxInt(int minLimit, int maxLimit)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
        public MinMaxInt(int minLimit, int maxLimit, int minValue, int maxValue)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
            MinValue = minValue;
            MaxValue = maxValue;
        }
        public int GetRandomValue()
        {
            return Random.Range(MinValue, MaxValue + 1);
        }
        public int GetRange()
        {
            return MaxValue - MinValue;
        }
    }
}