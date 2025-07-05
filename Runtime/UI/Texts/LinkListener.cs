using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HexTecGames.Basics.UI
{
    public class LinkListener : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private TMP_Text textGUI = default;
        [SerializeField, HideInInspector] private Camera cam = default;

        private List<TextData> textDatas;

        private int currentIndex = -1;

        public TMP_Text TextGUI
        {
            get
            {
                return this.textGUI;
            }
            set
            {
                this.textGUI = value;
            }
        }

        public event Action<LinkListener, TextData> OnLinkHover;
        public event Action<LinkListener> OnHoverStopped;


        private void Awake()
        {
            cam = Camera.main;
        }

        private void Reset()
        {
            cam = Camera.main;
            TextGUI = GetComponent<TMP_Text>();
        }

        public void Setup(List<TextData> textDatas)
        {
            this.textDatas = textDatas;
            enabled = true;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (textDatas == null)
            {
                return;
            }

            Vector2 mousePosition = eventData.position;
            int index = TMP_TextUtilities.FindIntersectingLink(TextGUI, mousePosition, cam);

            if (index == currentIndex)
            {
                return;
            }

            currentIndex = index;

            if (index < 0)
            {
                OnHoverStopped?.Invoke(this);
                return;
            }

            OnLinkHover?.Invoke(this, textDatas[currentIndex]);
        }


    }
}