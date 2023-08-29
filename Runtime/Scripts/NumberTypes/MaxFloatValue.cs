using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class MaxFloatValue : FloatValue
    {
        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                MaxValueChanged?.Invoke(maxValue);
            }
        }
        [SerializeField] private float maxValue = default;

        public override float Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (value > MaxValue)
                {
                    value = MaxValue;
                }
                base.Value = value;
            }
        }
        public Action<float> MaxValueChanged;

        public bool IsFull
        {
            get
            {
                return MaxValue <= Value;
            }
        }

        public float Percent
        {
            get
            {
                return Value / MaxValue;
            }
        }
        public void SetMaxAndCurrent(float value)
        {
            MaxValue = value;
            Value = value;
        }
        public void FillUp()
        {
            Value = MaxValue;
        }
    }
}