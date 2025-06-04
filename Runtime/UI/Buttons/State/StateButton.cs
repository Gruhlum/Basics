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
        [SerializeField] private TMP_Text textGUI = default;

        [SerializeField] private List<State> states = default;

        private int stateIndex = 0;

        private void Reset()
        {
            image = GetComponent<Image>();
            textGUI = GetComponentInChildren<TMP_Text>();
        }


        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
            if (states != null && states.Count >= 1)
            {
                LoadState(states[0]);
            }
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
            if (Application.isPlaying)
            {
                if (state != null)
                {
                    state.clickEvent?.Invoke();
                }
            }
        }
    }
}