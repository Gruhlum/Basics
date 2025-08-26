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
    public class ProgressBarAnimator : SliderAnimator
    {
        private enum OverflowType { None, Repeat, Increase }

        [SerializeField] private OverflowType overflow = default;
        [Space]
        [DrawIf(nameof(overflow), OverflowType.None, reverse: true)][SerializeField] private float waitTimeOnOverflow = 0.1f;
        [DrawIf(nameof(overflow), OverflowType.Increase)][SerializeField] private float flatIncreasePerLevel = 10f;
        [DrawIf(nameof(overflow), OverflowType.Increase)][SerializeField] private float expoIncreasePerLevel = 0.1f;

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

        private float startMaxValue;

        private float totalValue;

        public event Action<int> OnLevelChanged;


        private void Awake()
        {
            startMaxValue = MaxValue;
        }
        //[ContextMenu("Print Total Value")]
        //public void PrintTotalValue()
        //{
        //    float totalValue = 0;
        //    for (int i = 1; i < CurrentLevel; i++)
        //    {
        //        totalValue += CalculateRequiredPoints(i);
        //    }
        //    totalValue += TargetValue;
        //    Debug.Log("Total Value: " + totalValue + " : " + totalValue);
        //}

        //[ContextMenu("Print Increase")]
        //public void PrintIncrease()
        //{
        //    for (int i = 1; i < 51; i++)
        //    {
        //        Debug.Log($"Level {i} {CalculateRequiredPoints(i)}");
        //    }
        //}

        private float CalculateRequiredPoints(int level)
        {
            if (level < 1)
            {
                Debug.LogWarning("Level can't be negative!");
                return 0;
            }
            return startMaxValue + (flatIncreasePerLevel * level) + Mathf.Pow(expoIncreasePerLevel, level);
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
        protected override void SetTargetValue(float value)
        {
            totalValue = value;

            if (value > MaxValue)
            {
                HandleOverflow(value);
            }
            else base.SetTargetValue(value);
        }
        protected override void SetTargetValueInstantly(float value)
        {
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
            base.SetTargetValueInstantly(value);
        }
        
        protected override IEnumerator Animate()
        {
            int repeatsToAnimate = TotalRepeats - animatedRepeats;

            for (int i = 0; i < repeatsToAnimate; i++)
            {
                yield return AnimateFilling(RawSliderValue, MaxValue);
                animatedRepeats++;
                yield return new WaitForSeconds(waitTimeOnOverflow);
                RawSliderValue = 0;
                if (overflow == OverflowType.Increase)
                {
                    MaxValue = CalculateRequiredPoints(animatedRepeats);
                }
                //Debug.Log("here: " + animatedRepeats);
            }
            yield return AnimateFilling(RawSliderValue, TargetValue);
            animationCoroutine = null;
        }


    }
}