using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.EaseFunctions;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    [RequireComponent(typeof(Slider))]
    public class TimerAnimator : AdvancedBehaviour
    {
        [SerializeField] private Slider slider = default;

        [SerializeField] private EaseFunction animationEasing = default;

        private float startTime;
        private float targetTime;
        private float stepLength;
        private float stepTimer;

        private float rawTime;

        private float RawTime
        {
            get
            {
                return this.rawTime;
            }
            set
            {
                this.rawTime = value;
                slider.SetValueWithoutNotify(rawTime);
            }
        }


        private void Awake()
        {
            if (startTime == targetTime)
            {
                this.enabled = false;
            }
        }

        public void SetMaxTime(float maxTime)
        {
            slider.maxValue = maxTime;
        }

        public void ResetTimer()
        {
            targetTime = 0;
            RawTime = 0;
        }
        public void SetTime(float time)
        {
            targetTime = time;
            RawTime = time;
            this.enabled = false;
        }
        public void AddTime(float time)
        {
            targetTime += time;
            stepLength = time;
            startTime = RawTime;
            stepTimer = 0;
            this.enabled = true;
        }

        private void Update()
        {
            if (RawTime >= slider.maxValue)
            {
                return;
            }
            stepTimer += Time.deltaTime;
            RawTime = Mathf.Lerp(startTime, targetTime, animationEasing.GetValue(stepTimer / stepLength));
        }
    }
}