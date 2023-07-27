
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
        public MonoBehaviour TargetObject
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
        [SerializeField] private MonoBehaviour targetObject = default;

        public ValType Type
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
        [SerializeField] private ValType type = default;

        [Header("References")]
        [SerializeField] protected TextMeshProUGUI textGUI = default;
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
            Setup();
        }

        private void Update()
        {
            if (Application.isPlaying == false)
            {
                Setup();
            }
        }

        protected virtual void Setup()
        {
            if (TargetObject != null && Type != null && textGUI != null)
            {
                IntValue intValue = null;
                if (TargetObject is IDisplayable displayable)
                {
                    intValue = displayable.FindIntValue(Type);
                }            

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
                else Debug.LogWarning("Could not find IntValue (" + Type.name + ") on " + TargetObject);
            }
        }
        public void Clicked()
        {
            OnClicked?.Invoke(this);
        }

        private void ClearEventHandler()
        {
            if (TargetObject != null && Type != null && TargetObject is IDisplayable displayable)
            {
                IntValue intValue = displayable.FindIntValue(Type);

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