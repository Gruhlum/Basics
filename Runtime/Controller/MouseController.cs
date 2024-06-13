using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Contains information about any objects that are under the mouse button. 
    /// </summary>
    public class MouseController : MonoBehaviour
    {
        public enum ButtonType { None, Down, Clicked, Up }

        [SerializeField] private Camera mainCam = default;
        [SerializeField] private LayerMask uiLayer = 5;

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
       
        public bool PointerOverUI
        {
            get
            {
                return pointerOverUI;
            }
            set
            {
                pointerOverUI = value;
            }
        }
        private bool pointerOverUI = default;

        private void Reset()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            hoverGO = DetectGameObject();
            PointerOverUI = IsPointerOverUI();

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
                return ButtonType.Clicked;
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
        private bool IsPointerOverUI()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
        {
            for (int index = 0; index < eventSystemRaycastResults.Count; index++)
            {
                RaycastResult raycastResult = eventSystemRaycastResults[index];
                if (raycastResult.sortingLayer == uiLayer)
                {
                    pointerUIElement = raycastResult.gameObject;
                    return true;
                }
                //else Debug.Log(uiLayer + " - " + raycastResult.sortingLayer);
            }
            pointerUIElement = null;
            return false;
        }

        //Gets all event system raycast results of current mouse or touch position.
        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }   
    }
}