using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class MaxIntValueDisplay : IntValueDisplay
    {
        [SerializeField] private TextMeshProUGUI maxText = default;
        protected override void Setup()
        {
            base.Setup();
            MaxIntValue maxIntValue = null;
            if (TargetObject is IDisplayable displayable)
            {
                maxIntValue = displayable.FindIntValue(Type) as MaxIntValue;
            }
            if (maxIntValue == null)
            {
                return;
            }
            maxText.text = maxIntValue.MaxValue.ToString();
            if (Application.isPlaying)
            {
                maxIntValue.MaxValueChanged += IntValue_MaxValueChanged;
            }
        }

        private void IntValue_MaxValueChanged(int val)
        {
            maxText.text = val.ToString();
        }
    }
}