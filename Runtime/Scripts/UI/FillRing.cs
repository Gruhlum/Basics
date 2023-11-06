using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    public class FillRing : MonoBehaviour
    {
        [SerializeField] private Image img = default;
        [SerializeField] private TextMeshProUGUI textGUI = default;

        public float StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }
        [SerializeField] private float startTime = default;

        public float TotalTime
        {
            get
            {
                return totalTime;
            }
            set
            {
                totalTime = value;
            }
        }
        [SerializeField] private float totalTime = default;

        public bool ReachedThreshold
        {
            get
            {
                return currentTime >= StartTime;
            }
        }

        private float currentTime;

        private void Reset()
        {
            img = GetComponent<Image>();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            if (active)
            {
                ResetTime();
            }           
        }
        public void ResetTime()
        {
            currentTime = 0;
            img.fillAmount = 0;
        }
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public bool IncreaseTime(float time)
        {
            currentTime += time;
            if (currentTime < startTime)
            {
                return false;
            }
            if (textGUI != null)
            {
                textGUI.gameObject.SetActive(true);
            }           
            img.fillAmount = CalculatePercent(currentTime);
            return currentTime - startTime > TotalTime;
        }

        public void Setup(string text)
        {
            if (textGUI != null)
            {
                textGUI.gameObject.SetActive(false);
                textGUI.text = text;
            }
            SetActive(true);
        }

        public float CalculatePercent(float currentTime)
        {
            return Mathf.Lerp(0f, 1f, (currentTime - StartTime) / totalTime);
        }
    }
}