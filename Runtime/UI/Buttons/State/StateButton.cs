using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    public class StateButton : MonoBehaviour
    {
        [SerializeField] private Image image = default;
        [SerializeField] private Button btn = default;
        [SerializeField] private TMP_Text textGUI = default;

        [SerializeField] private bool addClickListener = true;
        [SerializeField] private List<State> states = default;

        private int stateIndex = 0;

        private void Reset()
        {
            image = GetComponent<Image>();
            btn = GetComponent<Button>();
            textGUI = GetComponentInChildren<TMP_Text>();
        }

        private void Awake()
        {
            btn.onClick.AddListener(ButtonClicked);
        }
        private void OnValidate()
        {
            if (states != null && states.Count >= 1)
            {
                LoadState(states[0]);
            }
        }
        private void ButtonClicked()
        {
            states[stateIndex].clickEvent?.Invoke();
            NextState();
            btn.interactable = false;
            btn.interactable = true;
        }
        public void SetState(int index)
        {
            if (states == null)
            {
                return;
            }
            if (index >= states.Count)
            {
                Debug.LogError("Index out of bounds!");
            }
            stateIndex = index;
            LoadState(states[stateIndex]);
        }
        public void NextState()
        {
            if (states == null || states.Count <= 1)
            {
                return;
            }
            stateIndex++;
            if (stateIndex >= states.Count)
            {
                stateIndex = 0;
            }
            SetState(stateIndex);
        }
        private void LoadState(State state)
        {
            if (image != null)
            {
                image.sprite = state.sprite;
            }
            if (textGUI != null)
            {
                textGUI.text = state.text;
            }
        }
    }
}