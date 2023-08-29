using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class StateController : MonoBehaviour
    {
        public static bool IsPaused;

        public static event Action<float> OnGameTick;

        private float timer;
        private float timeTick = 0.5f;

        protected virtual void FixedUpdate()
        {
            if (IsPaused)
            {
                return;
            }
            timer += Time.deltaTime;
            if (timer >= timeTick)
            {
                timer -= timeTick;
                OnGameTick?.Invoke(timeTick);
            }
        }      
    }
}