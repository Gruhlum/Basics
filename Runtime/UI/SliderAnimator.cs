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
        }
        public void AddRevolution(int amount = 1)
        {
            totalRevolutions += amount;
            if (fillAnimationCoroutine == null)
            {
                fillAnimationCoroutine = StartCoroutine(AnimateFill());
            }
        }

        public void SetValueInstantly(float value)
        {
            StopAnimationCoroutine();
            slider.value = value;
            targetValue = value;
        }

        public void SetValue(float value)
        {
            targetValue = value;

            if (canCancel)
            {
                StopAnimationCoroutine();
                fillAnimationCoroutine = StartCoroutine(AnimateFill());
            }  
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
            yield return AnimateBar(slider.value, targetValue);

            fillAnimationCoroutine = null;
        }

        private IEnumerator AnimateBar(float startValue, float targetValue)
        {
            float timer = 0;
            //float percentDistance = (targetValue - startValue) / (slider.maxValue - slider.minValue);
            //Debug.Log(percentDistance);
            //percentDistance = Mathf.Min(percentDistance, 0.8f);
            while (timer < 1)
            {
                float progress = easeFunction.GetValue(timer);
                slider.value = Mathf.Lerp(startValue, targetValue, progress);
                yield return null;
                timer += Time.deltaTime * speed;// * (1 - percentDistance);
            }
        }
    }
}