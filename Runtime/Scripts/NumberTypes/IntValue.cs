using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	[System.Serializable]
    public class IntValue
	{
        public ValType Type;
        public virtual int Value
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
        [SerializeField] private int value = default;

        public event Action<int> ValueChanged;
    }
}