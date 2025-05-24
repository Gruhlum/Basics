using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class TrackedInt<T> where T : TrackedInt<T>
    {
        public virtual int Value
        {
            get
            {
                return this.value;
            }
            private set
            {
                this.value = value;
                OnValueChanged?.Invoke((T)this, this.value);
            }
        }
        [SerializeField] private int value;

        public event Action<T, int> OnValueChanged;
    }
}