using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics.UI.Sliders.Tests
{
    public class HealthbarTester : AdvancedBehaviour
    {
        [SerializeField] private HealthbarAnimator healthbar = default;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                healthbar.AddValue(-20);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                healthbar.AddValue(50);
            }
        }
    }
}