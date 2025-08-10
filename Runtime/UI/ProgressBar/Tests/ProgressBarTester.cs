using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics.UI.Tests
{
    public class ProgressBarTester : AdvancedBehaviour
    {
        [SerializeField] private BetterProgressBar betterProgressBar = default;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                betterProgressBar.AddValue(2200);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                betterProgressBar.AddValue(50);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                betterProgressBar.AddValue(300);
            }
        }
    }
}