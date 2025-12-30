using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Centralized mouse controller that detects:
    /// - The GameObject currently hovered in the world (2D)
    /// - The UI element under the pointer
    /// - Whether the pointer is over a Selectable UI element
    /// - Mouse button states (Down, Held, Up)
    /// </summary>
    public class MouseController : AdvancedBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCam = default;

        [Header("Settings")]
        [SerializeField] private int uiLayer = 5;

        public GameObject PointerUIElement { get; private set; }

        public GameObject HoverGameObject { get; private set; }

        public ButtonType ButtonType { get; private set; } = ButtonType.None;

        public int ButtonNumber { get; private set; } = -1;

        public static bool PointerOverUI
        {
            get => pointerOverUI;
            private set
            {
                if (pointerOverUI == value) return;
                pointerOverUI = value;
                OnPointerOverUIChanged?.Invoke(value);
            }
        }
        private static bool pointerOverUI;

        public static bool PointerOverSelectable
        {
            get => pointerOverSelectable;
            private set
            {
                if (pointerOverSelectable == value) return;
                pointerOverSelectable = value;
                OnPointerOverSelectableChanged?.Invoke(value);
            }
        }
        private static bool pointerOverSelectable;

        public static event Action<bool> OnPointerOverUIChanged;
        public static event Action<bool> OnPointerOverSelectableChanged;

        protected override void Reset()
        {
            base.Reset();
            mainCam = Camera.main;
        }

        private void Update()
        {
            HoverGameObject = DetectWorldObject();
            DetectUI();

            DetectMouseButtons();
        }

        /// <summary>
        /// Returns true if the given button type and index occurred this frame.
        /// </summary>
        public bool IsButtonActive(ButtonType type, int index)
        {
            return (ButtonType == type && ButtonNumber == index);
        }

        /// <summary>
        /// Attempts to get a component from the hovered world object.
        /// </summary>
        public bool TryGetHoverObject<T>(out T component) where T : Component
        {
            if (HoverGameObject != null && HoverGameObject.TryGetComponent(out component))
                return true;

            component = null;
            return false;
        }

        private GameObject DetectWorldObject()
        {
            Vector2 worldPos = mainCam.GetMousePosition();
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            return hit.collider ? hit.collider.gameObject : null;
        }

        /// <summary>
        /// Detects UI elements under the pointer and updates PointerOverUI and PointerOverSelectable.
        /// </summary>
        private void DetectUI()
        {
            var results = GetEventSystemRaycastResults();

            if (results.Count == 0)
            {
                PointerUIElement = null;
                PointerOverUI = false;
                PointerOverSelectable = false;
                return;
            }

            var top = results[0];

            PointerOverUI = top.gameObject.layer == uiLayer;
            PointerUIElement = PointerOverUI ? top.gameObject : null;

            PointerOverSelectable = PointerOverUI && IsSelectable(top.gameObject);
        }

        private bool IsSelectable(GameObject go)
        {
            var selectable = go.GetComponentInParent<Selectable>();
            return selectable != null && selectable.interactable;
        }

        private void DetectMouseButtons()
        {
            ButtonType = ButtonType.None;
            ButtonNumber = -1;

            for (int i = 0; i < 2; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    ButtonType = ButtonType.Held;
                    ButtonNumber = i;
                    return;
                }
                if (Input.GetMouseButton(i))
                {
                    ButtonType = ButtonType.Down;
                    ButtonNumber = i;
                    return;
                }
                if (Input.GetMouseButtonUp(i))
                {
                    ButtonType = ButtonType.Up;
                    ButtonNumber = i;
                    return;
                }
            }
        }

        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results;
        }
    }
}
