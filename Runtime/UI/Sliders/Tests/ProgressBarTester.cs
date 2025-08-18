using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics.UI.Tests
{
    public class ProgressBarTester : AdvancedBehaviour
    {
        [SerializeField] private ProgressBarAnimator progressBarAnimator = default;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                progressBarAnimator.AddValue(50);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                progressBarAnimator.AddValue(100);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                progressBarAnimator.AddValue(150);
            }
        }
    }
}