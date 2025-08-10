using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.Basics.UI;
using HexTecGames.EaseFunctions;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    [RequireComponent(typeof(Slider))]
    public class BetterProgressBar : AdvancedBehaviour
    {
        private enum OverflowType { None, Repeat, Increase }

        [SerializeField] private Slider slider = default;

        [SerializeField] private EaseFunction sliderEasing = default;
        [SerializeField] private EaseFunction animationEasing = default;
        [SerializeField] private float animationSpeed = 5f;
        [SerializeField] private OverflowType overflow = default;
        [Space]
        [DrawIf(nameof(overflow), OverflowType.None, reverse: true)][SerializeField] private float waitTimeOnOverflow = 0.1f;
        [DrawIf(nameof(overflow), OverflowType.Increase)][SerializeField] private float flatIncreasePerLevel = 10f;
        [DrawIf(nameof(overflow), OverflowType.Increase)][SerializeField] private float expoIncreasePerLevel = 0.1f;

        public float MaxValue
        {
            get
            {
                return slider.maxValue;
            }
            private set
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
            private set
            {
                rawSliderValue = value;
                float percent = value / MaxValue;
                float progress = sliderEasing.GetValue(percent);
                slider.SetValueWithoutNotify(progress * MaxValue);
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
            private set
            {
                this.targetValue = value;
                OnTargetValueChanged?.Invoke(value);
            }
        }

        public int CurrentLevel
        {
            get
            {
                return TotalRepeats + 1;
            }
        }

        public int TotalRepeats
        {
            get
            {
                return this.totalRepeats;
            }

            private set
            {
                this.totalRepeats = value;
                OnLevelChanged?.Invoke(value + 1);
            }
        }

        private int totalRepeats;
        private int animatedRepeats;
        private float targetValue;

        private float startMaxValue;
        private Coroutine animationCoroutine;

        private float totalValue;

        public event Action<float> OnTargetValueChanged;
        public event Action<int> OnLevelChanged;


        protected override void Reset()
        {
            slider = GetComponent<Slider>();
        }

        private void Awake()
        {
            startMaxValue = MaxValue;
        }
        [ContextMenu("Print Total Value")]
        public void PrintTotalValue()
        {
            float totalValue = 0;
            for (int i = 1; i < CurrentLevel; i++)
            {
                totalValue += CalculateRequiredPoints(i);
            }
            totalValue += TargetValue;
            Debug.Log("Total Value: " + totalValue + " : " + totalValue);
        }

        [ContextMenu("Print Increase")]
        public void PrintIncrease()
        {
            for (int i = 1; i < 51; i++)
            {
                Debug.Log($"Level {i} {CalculateRequiredPoints(i)}");
            }
        }

        private float CalculateRequiredPoints(int level)
        {
            if (level < 1)
            {
                Debug.LogWarning("Level can't be negative!");
                return 0;
            }
            return startMaxValue + (flatIncreasePerLevel * level) + Mathf.Pow(expoIncreasePerLevel, level);
        }
        public void AddValue(float value)
        {
            SetValue(TargetValue + value);
        }
        public void AddValueInstantly(float value)
        {
            SetValueInstantly(TargetValue + value);
        }
        public void SetValue(float value)
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
            totalValue = value;

            if (value > MaxValue)
            {
                HandleOverflow(value);
            }
            else TargetValue = value;

            animationCoroutine = StartCoroutine(Animate());
        }

        public float GetPercentProgress(float value)
        {
            return value / MaxValue;
        }

        private void HandleOverflow(float value)
        {
            if (overflow == OverflowType.Increase)
            {
                int currentRepeats = 0;
                while (value > MaxValue)
                {
                    float targetMaxValue = CalculateRequiredPoints(CurrentLevel + currentRepeats);
                    value -= targetMaxValue;
                    currentRepeats++;
                }
                TotalRepeats += currentRepeats;
                TargetValue = value;
            }
            else if (overflow == OverflowType.Repeat)
            {
                TotalRepeats += Mathf.FloorToInt(value / MaxValue);
                TargetValue = value % MaxValue;
            }
        }

        public void SetValueInstantly(float value)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
            if (overflow == OverflowType.Repeat)
            {
                if (value > MaxValue)
                {
                    TotalRepeats = Mathf.FloorToInt(value / MaxValue);
                    value %= MaxValue;
                }
            }
            else value = Mathf.Min(MaxValue, value);
            totalValue = value;
            SliderValue = value;
            TargetValue = value;
        }

        private IEnumerator Animate()
        {
            int repeatsToAnimate = TotalRepeats - animatedRepeats;

            for (int i = 0; i < repeatsToAnimate; i++)
            {
                yield return AnimateFilling(rawSliderValue, MaxValue);
                animatedRepeats++;
                yield return new WaitForSeconds(waitTimeOnOverflow);
                SliderValue = 0;
                if (overflow == OverflowType.Increase)
                {
                    MaxValue = CalculateRequiredPoints(animatedRepeats);
                }
                //Debug.Log("here: " + animatedRepeats);
            }
            yield return AnimateFilling(rawSliderValue, targetValue);
            animationCoroutine = null;
        }

        private IEnumerator AnimateFilling(float startValue, float targetValue)
        {
            float percentChange = Mathf.Clamp01((targetValue - startValue) / MaxValue);
            float duration = 1f / animationSpeed * percentChange;
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / duration);
                float easedProgress = animationEasing.GetValue(progress);
                //Debug.Log($"{nameof(progress)} {progress} + {nameof(easedProgress)} {easedProgress}");
                SliderValue = Mathf.LerpUnclamped(startValue, targetValue, easedProgress);
                yield return null;
            }
            SliderValue = targetValue;
        }
    }
}