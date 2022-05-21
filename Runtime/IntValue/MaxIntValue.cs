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
                Debug.Log("hi");
                ValChanged.Invoke(value.ToString());
            }
        }

        public UnityEvent<string> ValChanged;
    }
}