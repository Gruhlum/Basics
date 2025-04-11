using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.EaseFunctions;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class SliderAnimator : MonoBehaviour
    {
        public Slider Slider
        {
            get
            {
                return slider;
            }
        }
        [SerializeField] private Slider slider = default;

        [SerializeField] private EaseFunction easeFunction = default;
        [SerializeField] private float speed = 10;
        [SerializeField] private float waitTimeOnFull = 0.1f;

        private float targetValue;
        private Coroutine fillAnimationCoroutine;
        private Coroutine subCoroutine;
        private int totalRevolutions;
        private bool canCancel = true;

        private void Reset()
        {
            slider = GetComponent<Slider>();
        }
        private void StopAnimationCoroutine()
        {
            if (fillAnimationCoroutine != null)
            {
                StopCoroutine(fillAnimationCoroutine);
            }
            if (subCoroutine != null)
            {
                StopCoroutine(subCoroutine);
            }
        }

        public float GetPercentProgress()
        {
            return GetPercentProgress(Slider.value);
        }
        public float GetPercentProgress(float value)
        {
            float difference = Slider.maxValue - Slider.minValue;
            return value / difference;
        }

        public void AddRevolution(int amount = 1)
        {
            totalRevolutions += amount;
            if (fillAnimationCoroutine == null)
            {
                canCancel = false;
                fillAnimationCoroutine = StartCoroutine(AnimateFill());
            }
        }

        private void SetValueInstantly(float value)
        {
            float range = Slider.maxValue - Slider.minValue;
            float progress = value / range;
            float currentValue = easeFunction.GetValue(progress) * range;
            targetValue = currentValue;

            if (canCancel)
            {
                StopAnimationCoroutine();
            }
            slider.value = currentValue;
        }

        private void SetValue(float value)
        {
            targetValue = value;
            if (canCancel)
            {
                StopAnimationCoroutine();
                fillAnimationCoroutine = StartCoroutine(AnimateFill());
            }
        }
        public void SetValue(float value, bool instantly = false)
        {
            if (instantly)
            {
                SetValueInstantly(value);
            }
            else SetValue(value);
        }

        private IEnumerator AnimateFill()
        {
            while (totalRevolutions > 0)
            {
                canCancel = false;
                yield return AnimateBar(slider.value, slider.maxValue);
                yield return new WaitForSeconds(waitTimeOnFull);
                totalRevolutions--;
                slider.value = slider.minValue;
            }
            canCancel = true;
            fillAnimationCoroutine = null;

            subCoroutine = StartCoroutine(AnimateBar(slider.value, targetValue));
        }

        private IEnumerator AnimateBar(float startValue, float targetValue)
        {
            float timer = 0;
            //float percentDistance = (targetValue - startValue) / (slider.maxValue - slider.minValue);
            //Debug.Log(percentDistance);
            //percentDistance = Mathf.Min(percentDistance, 0.8f);
            while (timer < 1)
            {
                timer += Time.deltaTime * speed;// * (1 - percentDistance);
                float progress = easeFunction.GetValue(timer);
                slider.value = Mathf.Lerp(startValue, targetValue, progress);
                yield return null;
                //Debug.Log(progress + " - " + slider.value + " - " + targetValue);
            }
            subCoroutine = null;
        }
    }
}