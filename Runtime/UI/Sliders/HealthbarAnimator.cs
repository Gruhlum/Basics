using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Sliders
{
    public class HealthbarAnimator : SliderAnimator
    {
        [InspectorOrder(1, "References")][SerializeField] private Image backgroundFill = default;
        [InspectorOrder(2, "References")][SerializeField] private Image centerFill = default;
        [InspectorOrder(3, "References")][SerializeField] private Image foregroundFill = default;
        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.green;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color healColor = Color.cyan;

        private void Awake()
        {
            foregroundFill.fillAmount = 1;
            RawSliderValue = MaxValue;
            TargetValue = MaxValue;
            foregroundFill.color = normalColor;
            backgroundFill.color = healColor;
        }

        private void OnValidate()
        {
            if (centerFill != null)
            {
                centerFill.color = normalColor;
            }
        }

        protected override void SetValueInstantly(float value)
        {
            base.SetValueInstantly(value);
            float progress = sliderEasing.GetValue(value / MaxValue);
            backgroundFill.fillAmount = progress;
            foregroundFill.fillAmount = progress;
        }

        protected override void SetValue(float value)
        {
            float progress = sliderEasing.GetValue(value / MaxValue);

            if (value > TargetValue) // We are Healing
            {
                backgroundFill.fillAmount = progress;
                foregroundFill.fillAmount = 0;
                centerFill.color = normalColor;
            }
            else
            {
                foregroundFill.fillAmount = progress;
                backgroundFill.fillAmount = 0;
                centerFill.color = damageColor;
            }
            base.SetValue(value);
        }
    }
}