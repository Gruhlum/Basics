using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics.UI.Sliders.Tests
{    
    public class TimerTester : AdvancedBehaviour
    {
        [SerializeField] private TimerAnimator timerAnimator = default;

        [SerializeField] private float maxTime = 30;
        [SerializeField] private float tickLength = 0.5f;

        private float timer;

        private void Start()
        {
            timerAnimator.SetMaxTime(maxTime);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= tickLength)
            {
                timer -= tickLength;
                timerAnimator.AddTime(tickLength);
            }
        }
    }
}