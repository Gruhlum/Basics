using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HexTecGames.Basics
{
    [System.Serializable]
	public class MaxIntValue : IntValue
	{
        public int MaxValue
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
        [SerializeField] private int maxValue = default;

        public override int Value
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
        public Action<int> MaxValueChanged;

        public bool IsFull
        {
            get
            {
                return MaxValue == Value;
            }
        }

        public float Percent
        {
            get
            {
                return (float)Value / (float)MaxValue;
            }
        }
        public void SetMaxAndCurrent(int value)
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