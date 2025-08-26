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
                //RawSliderValue = TargetValue;
            }
           
            //SetTargetValue(value);
            //if (animationCoroutine == null)
            //{
            //    animationCoroutine = StartCoroutine(Animate());
            //}
            base.SetValue(value);
        }

        //protected override IEnumerator AnimateFilling(float startValue, float targetValue)
        //{
        //    float percentChange = (TargetValue - startValue) / MaxValue;
        //    float duration = 1f / animationSpeed * Mathf.Abs(percentChange);
        //    float timer = 0;

        //    //Debug.Log($"{nameof(startValue)} {startValue} + {nameof(targetValue)} {targetValue}");
        //    while (timer < duration)
        //    {
        //        timer += Time.deltaTime;
        //        float progress = Mathf.Clamp01(timer / duration);
        //        float easedProgress = animationEasing.GetValue(progress);
        //        //Debug.Log($"{nameof(progress)} {progress} + {nameof(easedProgress)} {easedProgress}");
        //        RawSliderValue = Mathf.LerpUnclamped(startValue, TargetValue, easedProgress);
        //        yield return null;
        //    }
        //    RawSliderValue = TargetValue;
        //}
    }
}