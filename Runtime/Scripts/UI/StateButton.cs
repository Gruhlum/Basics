using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    [RequireComponent(typeof(Button))]
    public class StateButton : MonoBehaviour
    {
        [SerializeField] private Button btn = default;
        [SerializeField] private TextMeshProUGUI textGUI = default;

        public List<string> States;
        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
            set
            {
                currentIndex = value;
            }
        }
        private int currentIndex;

        public event Action<int> OnButtonClicked;


        private void OnValidate()
        {
            if (textGUI != null && States != null && States.Count > 0)
            {
                textGUI.text = States[0];
            }
        }

        private void Reset()
        {
            btn = GetComponent<Button>();
            textGUI = GetComponentInChildren<TextMeshProUGUI>();
            States = new List<string>();
            if (textGUI != null)
            {
                States.Add(textGUI.text);
            }
        }
        private void Awake()
        {
            btn.onClick.AddListener(delegate { ButtonClicked(); });
        }

        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke(CurrentIndex);
            CurrentIndex++;
            if (CurrentIndex >= States.Count)
            {
                CurrentIndex = 0;
            }
            textGUI.text = States[CurrentIndex];
        }
    }
}