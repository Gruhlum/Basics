using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    public class RadioButtonController : MonoBehaviour
    {
        [SerializeField] private List<RadioButton> buttons = default;

        [SerializeField] private RadioButton startButton = default;

        public event Action<RadioButton> OnRadioButtonClicked;

        private void Reset()
        {
            buttons = GetComponentsInChildren<RadioButton>().ToList();
        }

        private void Awake()
        {
            foreach (var button in buttons)
            {
                button.OnClicked += Button_OnClicked;
            }
        }
        private void Start()
        {
            if (startButton != null)
            {
                startButton.SetActive(true);
            }
        }

        private void Button_OnClicked(RadioButton radioBtn)
        {
            foreach (var btn in buttons)
            {
                btn.SetActive(btn == radioBtn);
            }
            OnRadioButtonClicked?.Invoke(radioBtn);
        }
    }
}