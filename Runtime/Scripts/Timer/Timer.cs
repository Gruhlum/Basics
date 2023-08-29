using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class Timer : MonoBehaviour
	{
        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        [SerializeField] private float value = 1;

        private float currentTime;

        public event Action Expired;

        private void FixedUpdate()
        {
            currentTime += Time.deltaTime;
            if (currentTime >= Value)
            {
                Expired?.Invoke();
                currentTime = 0;
            }
        }
    }
}