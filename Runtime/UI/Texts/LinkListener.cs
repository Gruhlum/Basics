using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HexTecGames.Basics.UI
{
    public class LinkListener : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private TMP_Text textGUI = default;
        [SerializeField, HideInInspector] private Camera cam = default;

        private List<string> linkTexts;

        private int currentIndex = -1;

        public event Action<string> OnLinkHover;
        public event Action OnHoverStopped;


        private void Awake()
        {
            cam = Camera.main;
        }

        private void Reset()
        {
            cam = Camera.main;
            textGUI = GetComponent<TMP_Text>();
        }

        public void Setup(List<string> linkTexts)
        {
            this.linkTexts = linkTexts;
            enabled = true;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (linkTexts == null)
            {
                return;
            }

            Vector2 mousePosition = eventData.position;
            int index = TMP_TextUtilities.FindIntersectingLink(textGUI, mousePosition, cam);

            if (index == currentIndex)
            {
                return;
            }

            currentIndex = index;

            if (index < 0)
            {
                OnHoverStopped?.Invoke();
                return;
            }

            OnLinkHover?.Invoke(linkTexts[currentIndex]);
        }


    }
}