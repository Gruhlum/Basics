using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private float speed = 10;

        private float targetValue;
        private bool stopAnimation;

        private void Reset()
        {
            slider = GetComponent<Slider>();
        }
        private void Awake()
        {
            targetValue = slider.value;
        }

        public void SetValue(float value, bool instantly = false)
        {
            if (instantly)
            {
                Slider.value = value;
            }
            targetValue = value;
        }
        public void StartFillAndReset(float duration, float remainder = 0f, float waitTime = 0.1f)
        {
            StartCoroutine(FillAndReset(duration, remainder, waitTime));
        }
        private void Update()
        {
            if (!stopAnimation && !Mathf.Approximately(targetValue, slider.value))
            {
                slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * speed);
            }
        }
        private IEnumerator FillAndReset(float duration, float remainder, float waitTime)
        {
            stopAnimation = true;
            //Debug.Log("Start Fill");
            float timer = 0;
            float startValue = slider.value;
            while (timer <= duration)
            {
                yield return null;
                timer += Time.deltaTime;
                slider.value = Mathf.Lerp(startValue, slider.maxValue, timer / duration);

            }
            yield return new WaitForSeconds(waitTime);
            slider.value = 0;
            targetValue = Mathf.Max(0, remainder);
            stopAnimation = false;
        }
    }
}