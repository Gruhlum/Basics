using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class MinMaxFloat
    {
        public float MinValue;
        public float MaxValue;
        public float MinLimit;
        public float MaxLimit;

        public MinMaxFloat()
        {
            MaxLimit = 10;
        }
        public MinMaxFloat(float minLimit, float maxLimit)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
        public MinMaxFloat(float minLimit, float maxLimit, float minValue, float maxValue)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
            MinValue = minValue;
            MaxValue = maxValue;
        }
        public float GetRandomValue()
        {
            return Random.Range(MinValue, MaxValue);
        }
        public float GetRange()
        {
            return MaxValue - MinValue;
        }
    }
}