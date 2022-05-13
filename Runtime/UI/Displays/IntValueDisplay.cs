
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    [ExecuteAlways]
    public class IntValueDisplay : MonoBehaviour
    {
        public Displayable TargetObject
        {
            get
            {
                return targetObject;
            }
            set
            {
                if (targetObject != null)
                {
                    ClearEventHandler();
                }

                targetObject = value;

                if (targetObject != null)
                {
                    Setup();
                }
            }
        }
        [SerializeField] private Displayable targetObject = default;

        public ValueType Type
        {
            get
            {
                return type;
            }
            set
            {
                if (type != null)
                {
                    ClearEventHandler();
                }

                type = value;

                if (type != null)
                {
                    Setup();
                }
            }
        }
        [SerializeField] private ValueType type = default;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI textGUI = default;
        [SerializeField] private Image image = default;

        public event Action<IntValueDisplay> OnClicked;

        private void Reset()
        {
            if (textGUI == null)
            {
                textGUI = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void Awake()
        {
            //textGUI.text = player.Item.IntValues.Find(x => x.Type == Type).ToString();
        }

        private void Update()
        {
            if (Application.isPlaying == false)
            {
                Setup();
            }
        }

        private void Setup()
        {
            if (TargetObject != null && Type != null && textGUI != null)
            {
                IntValue intValue = TargetObject.IntValues.Find(Type);

                if (intValue != null)
                {
                    if (Application.isPlaying)
                    {
                        intValue.ValueChanged += IntValue_ValueChanged;
                    }
                    
                    textGUI.text = intValue.Value.ToString();
                    if (image != null)
                    {
                        image.sprite = intValue.Type.Sprite;
                    }                   
                }
                else Debug.LogWarning("Could not find IntValue (" + Type.name + ") on " + TargetObject.name);
            }
        }

        public void Clicked()
        {
            OnClicked?.Invoke(this);
        }

        private void ClearEventHandler()
        {
            if (TargetObject != null && Type != null)
            {
                IntValue intValue = TargetObject.IntValues.Find(Type);

                if (intValue != null)
                {
                    intValue.ValueChanged -= IntValue_ValueChanged;
                }
            }
        }
        private void IntValue_ValueChanged(int value)
        {
            textGUI.text = value.ToString();
        }
    }
}