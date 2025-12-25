using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Contains information about any objects that are under the mouse button.
    /// </summary>
    public class MouseController : AdvancedBehaviour
    {
        [SerializeField] private Camera mainCam = default;
        [SerializeField] private int uiLayer = 5;

        public GameObject PointerUIElement
        {
            get
            {
                return pointerUIElement;
            }
            private set
            {
                pointerUIElement = value;
            }
        }
        private GameObject pointerUIElement;


        public GameObject HoverGO
        {
            get
            {
                return hoverGO;
            }
            private set
            {
                hoverGO = value;
            }
        }
        private GameObject hoverGO = default;

        public ButtonType BtnType
        {
            get
            {
                return btnType;
            }
            private set
            {
                btnType = value;
            }
        }
        private ButtonType btnType = default;

        public int BtnNumber
        {
            get
            {
                return btnNumber;
            }
            private set
            {
                btnNumber = value;
            }
        }
        private int btnNumber = -1;

        public static bool PointerOverUI
        {
            get
            {
                return pointerOverUI;
            }
            set
            {
                if (pointerOverUI == value)
                {
                    return;
                }
                pointerOverUI = value;
                OnPointerOverUIChanged?.Invoke(pointerOverUI);
            }
        }
        private static bool pointerOverUI = default;

        public static bool PointerOverSelectable
        {
            get
            {
                return pointerOverSelectable;
            }
            private set
            {
                if (pointerOverSelectable == value)
                {
                    return;
                }
                pointerOverSelectable = value;
                OnPointerOverSelectableChanged?.Invoke(pointerOverSelectable);
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
            HoverGO = DetectGameObject();
            DetectUI();

            for (int i = 0; i < 2; i++)
            {
                ButtonType type = CheckButton(i);
                if (type != ButtonType.None)
                {
                    BtnType = type;
                    BtnNumber = i;
                    return;
                }
            }
            BtnType = ButtonType.None;
            btnNumber = -1;
        }

        /// <summary>
        /// Checks if a specific button state has been active this frame.
        /// </summary>
        /// <param name="type">The state of the button (Clicked, Down, Up).</param>
        /// <param name="index">Index of the mouse button (0 = left, 1 = right).</param>
        public bool IsButtonActive(ButtonType type, int index)
        {
            if (BtnType == type && BtnNumber == index)
            {
                return true;
            }
            return false;
        }

        private ButtonType CheckButton(int btn)
        {
            if (Input.GetMouseButtonDown(btn))
            {
                return ButtonType.Held;
            }
            if (Input.GetMouseButton(btn))
            {
                return ButtonType.Down;
            }
            if (Input.GetMouseButtonUp(btn))
            {
                return ButtonType.Up;
            }
            return ButtonType.None;
        }
        public bool TryGetHoverObject<T>(out T t) where T : Component
        {
            if (HoverGO == null)
            {
                t = null;
                return false;
            }
            if (HoverGO.TryGetComponent(out t))
            {
                return true;
            }
            t = null;
            return false;
        }
        private GameObject DetectGameObject()
        {
            Vector2 worldPos = mainCam.GetMousePosition();
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        /// <summary>
        /// Checks if the mouse is hovering over an UI Element
        /// </summary>
        private void DetectUI()
        {
            var raycastResults = GetEventSystemRaycastResults();
            PointerOverUI = IsPointerOverUIElement(raycastResults);
            PointerOverSelectable = IsPointerOverSelectable(raycastResults);
        }
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
        {
            for (int index = 0; index < eventSystemRaycastResults.Count; index++)
            {
                RaycastResult raycastResult = eventSystemRaycastResults[index];

                if (raycastResult.gameObject.layer == uiLayer)
                {
                    PointerUIElement = raycastResult.gameObject;
                    return true;
                }
            }
            PointerUIElement = null;
            return false;
        }
        private bool IsPointerOverSelectable(List<RaycastResult> eventSystemRaycastResults)
        {
            for (int index = 0; index < eventSystemRaycastResults.Count; index++)
            {
                RaycastResult raycastResult = eventSystemRaycastResults[index];

                if (raycastResult.gameObject.layer != uiLayer)
                {
                    continue;
                }
                if (raycastResult.gameObject.TryGetComponent(out Selectable selectable) && selectable.interactable)
                {
                    return true;
                }
            }
            return false;
        }

        //Gets all event system raycast results of current mouse or touch position.
        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }
    }
}