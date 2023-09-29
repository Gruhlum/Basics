using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
	public class RadioButton : MonoBehaviour
	{
		[SerializeField] private Button btn = default;
        [SerializeField] private Image img = default;
     

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                if (active == value)
                {
                    return;
                }
                active = value;

                if (active)
                {
                    img.color = activeColor;
                }
                else img.color = normalColor;
            }
        }
        [SerializeField] private bool active = default;


        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color activeColor = Color.white;

        public event Action<RadioButton> OnClicked;


        private void Reset()
        {
            btn = GetComponent<Button>();
            img = GetComponent<Image>();
        }

        private void Awake()
        {
            btn.onClick.AddListener(delegate { OnButtonClicked(); });
        }
        private void OnButtonClicked()
        {
            OnClicked?.Invoke(this);
        }
    }
}