using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	[System.Serializable]
	public class FloatValue
	{
        public virtual float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                ValueChanged?.Invoke(this.value);
            }
        }
        [SerializeField] private float value = default;

        public event Action<float> ValueChanged;
    }
}