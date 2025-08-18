using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics.UI.ProgressBars
{    
    public class SliderTester : AdvancedBehaviour
    {
        [SerializeField] private SliderAnimator sliderAnimator = default;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                sliderAnimator.AddValue(-50);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                sliderAnimator.AddValue(50);
            }
        }
    }
}