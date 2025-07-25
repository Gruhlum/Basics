using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float scrollSpeed = 10f;
        private bool mouseOver = false;

        private List<Selectable> m_Selectables = new List<Selectable>();
        private ScrollRect m_ScrollRect;

        private Vector2 m_NextScrollPosition = Vector2.up;

        private void OnEnable()
        {
            if (m_ScrollRect)
            {
                m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
            }
        }
        private void Awake()
        {
            m_ScrollRect = GetComponent<ScrollRect>();
        }
        private void Start()
        {
            if (m_ScrollRect)
            {
                m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
            }
            ScrollToSelected(true);
        }
        private void Update()
        {
            // Scroll via input.
            ScrollToSelected(false);
            //InputScroll();
            if (!mouseOver)
            {
                // Lerp scrolling code.
                m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
        }
        private void InputScroll()
        {
            Debug.Log(Input.GetAxis("Vertical"));
            if (m_Selectables.Count > 0)
            {
                if (Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f || Input.GetButtonDown("Horizontal")
                    || Input.GetButtonDown("Vertical") || Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                {
                    ScrollToSelected(false);
                }
            }
        }
        private void ScrollToSelected(bool quickScroll)
        {
            int selectedIndex = -1;
            Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;
            //Debug.Log(selectedElement + " - " + EventSystem.current.currentSelectedGameObject);
            if (selectedElement)
            {
                selectedIndex = m_Selectables.IndexOf(selectedElement);
            }
            //Debug.Log(selectedElement + " - " + EventSystem.current.currentSelectedGameObject + " - " + selectedIndex);
            if (selectedIndex > -1)
            {
                if (quickScroll)
                {
                    m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                    m_NextScrollPosition = m_ScrollRect.normalizedPosition;
                }
                else
                {
                    m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                }
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseOver = true;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            mouseOver = false;
            ScrollToSelected(false);
        }
    }
}