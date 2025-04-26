using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class NumericUpDown : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentNumberGUI = default;
        [Space]
        [SerializeField] private Selectable leftButton = default;
        [SerializeField] private Selectable rightButton = default;

        public int MinNumber
        {
            get
            {
                return minNumber;
            }
            private set
            {
                minNumber = value;
            }
        }
        [Space][SerializeField] private int minNumber;

        public int MaxNumber
        {
            get
            {
                return maxNumber;
            }
            private set
            {
                maxNumber = value;
            }
        }
        [SerializeField] private int maxNumber = 1;

        public int CurrentNumber
        {
            get
            {
                return currentNumber;
            }
            private set
            {
                currentNumber = value;
            }
        }
        private int currentNumber;

        public event Action<int> OnCurrentNumberChanged;

        protected virtual void OnEnable()
        {
            SetCurrentNumber(CurrentNumber);
        }

        public int ClampNumber(int value)
        {
            return Mathf.Clamp(value, MinNumber, MaxNumber);
        }

        protected bool IsMinValue(int value)
        {
            return value <= MinNumber;
        }
        protected bool IsMaxValue(int value)
        {
            return value >= MaxNumber;
        }
        public void SetMaxNumber(int value)
        {
            MaxNumber = value;
            SetCurrentNumber(ClampNumber(CurrentNumber));
        }
        public void SetMinNumber(int value)
        {
            MinNumber = value;
            SetCurrentNumber(ClampNumber(value));
        }
        public void SetCurrentNumber(int value)
        {
            value = ClampNumber(value);

            CurrentNumber = value;
            currentNumberGUI.text = CurrentNumber.ToString();
            SetButtonInteractable(value);

            OnCurrentNumberChanged?.Invoke(value);
        }
        protected virtual void SetButtonInteractable(int value)
        {
            if (leftButton != null)
            {
                leftButton.interactable = !IsMinValue(value);
            }
            if (rightButton != null)
            {
                rightButton.interactable = !IsMaxValue(value);
            }
        }
        public void MoveLeft()
        {
            if (IsMinValue(CurrentNumber))
            {
                return;
            }
            SetCurrentNumber(--CurrentNumber);
        }
        public void MoveRight()
        {
            if (IsMaxValue(CurrentNumber))
            {
                return;
            }
            SetCurrentNumber(++CurrentNumber);
        }
    }
}