using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    public class FillRing : AdvancedBehaviour
    {
        [SerializeField] private Image img = default;
        [SerializeField] private TextMeshProUGUI timerGUI = default;

        public float StartThreshold
        {
            get
            {
                return startThreshold;
            }
            set
            {
                startThreshold = value;
            }
        }
        [Tooltip("The time it takes for the ring to become visible")][SerializeField] private float startThreshold = default;

        public float TargetTime
        {
            get
            {
                return targetTime;
            }
            set
            {
                targetTime = value;
            }
        }
        [SerializeField] private float targetTime = 2f;

        public bool ReachedThreshold
        {
            get
            {
                return currentTime >= StartThreshold;
            }
        }

        private float currentTime;

        public UnityEvent OnComplete;
        [SerializeField] private bool disableAfterCompletion = default;


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

        public void IncreaseTime(float time)
        {
            currentTime += time;
            if (currentTime < startThreshold)
            {
                return;
            }
            if (timerGUI != null)
            {
                timerGUI.gameObject.SetActive(true);
            }
            img.fillAmount = CalculatePercent(currentTime);
            if (currentTime >= TargetTime)
            {
                ResetTime();
                Complete();
            }
        }
        
        protected virtual void Complete()
        {
            if (disableAfterCompletion)
            {
                gameObject.SetActive(false);
            }

            OnComplete?.Invoke();
        }

        public void Setup(string text)
        {
            if (timerGUI != null)
            {
                timerGUI.gameObject.SetActive(false);
                timerGUI.text = text;
            }
            SetActive(true);
        }

        public float CalculatePercent(float currentTime)
        {
            return Mathf.Lerp(0f, 1f, (currentTime - StartThreshold) / targetTime);
        }
    }
}