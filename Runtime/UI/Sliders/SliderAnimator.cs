using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.EaseFunctions;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderAnimator : AdvancedBehaviour
    {
        [SerializeField] protected Slider slider = default;

        [SerializeField] private EaseFunction sliderEasing = default;
        [SerializeField] private EaseFunction animationEasing = default;
        [SerializeField] private float animationSpeed = 5f;

        public float MinValue
        {
            get
            {
                return slider.minValue;
            }
            set
            {
                slider.minValue = value;
            }
        }
        public float MaxValue
        {
            get
            {
                return slider.maxValue;
            }
            set
            {
                slider.maxValue = value;
            }
        }
        public float SliderValue
        {
            get
            {
                return slider.value;
            }
            set
            {
                slider.SetValueWithoutNotify(value);
            }
        }

        public float RawSliderValue
        {
            get
            {
                return rawSliderValue;
            }
            protected set
            {
                rawSliderValue = value;
                float percent = value / MaxValue;
                float progress = sliderEasing.GetValue(percent);
                SliderValue = progress * MaxValue;
                //slider.SetValueWithoutNotify(value);
            }
        }
        private float rawSliderValue;
        public float TargetValue
        {
            get
            {
                return this.targetValue;
            }
            protected set
            {
                value = Mathf.Clamp(value, MinValue, MaxValue);
                this.targetValue = value;
                OnTargetValueChanged?.Invoke(value);
            }
        }
        private float targetValue;

        protected Coroutine animationCoroutine;


        public event Action<float> OnTargetValueChanged;

        protected override void Reset()
        {
            slider = GetComponent<Slider>();
        }
        public float GetPercentProgress(float value)
        {
            return value / MaxValue;
        }

        public void AddValue(float value, bool instantly = false)
        {
            if (instantly)
            {
                AddValueInstantly(value);
            }
            else AddValue(value);
        }
        private void AddValue(float value)
        {
            SetValue(TargetValue + value);
        }
        private void AddValueInstantly(float value)
        {
            SetValueInstantly(TargetValue + value);
        }
        public void SetValue(float value, bool instantly = false)
        {
            if (instantly)
            {
                SetValueInstantly(value);
            }
            else SetValue(value);
        }
        protected virtual void SetValue(float value)
        {
            StopCurrentAnimation();
            SetTargetValue(value);

            animationCoroutine = StartCoroutine(Animate());
        }

        protected virtual void SetTargetValue(float value)
        {
            TargetValue = value;
        }
        protected virtual void SetTargetValueInstantly(float value)
        {
            SliderValue = value;
            TargetValue = value;
        }
        protected virtual void SetValueInstantly(float value)
        {
            StopCurrentAnimation();
            SetTargetValueInstantly(value);
        }

        private void StopCurrentAnimation()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }
        protected virtual IEnumerator Animate()
        {
            yield return AnimateFilling(RawSliderValue, TargetValue);
            animationCoroutine = null;
        }

        protected IEnumerator AnimateFilling(float startValue, float targetValue)
        {
            float percentChange = (targetValue - startValue) / MaxValue;
            float duration = 1f / animationSpeed * Mathf.Abs(percentChange);
            float timer = 0;
            Debug.Log($"{nameof(startValue)} {startValue} + {nameof(targetValue)} {targetValue}");
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / duration);
                float easedProgress = animationEasing.GetValue(progress);
                //Debug.Log($"{nameof(progress)} {progress} + {nameof(easedProgress)} {easedProgress}");
                RawSliderValue = Mathf.LerpUnclamped(startValue, targetValue, easedProgress);
                yield return null;
            }
            RawSliderValue = targetValue;
        }
    }
}