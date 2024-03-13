using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HexTecGames.Basics
{
    public class MouseController : MonoBehaviour
    {
        public enum ButtonType { None, Down, Clicked, Up }

        [SerializeField] private Camera mainCam = default;

        public GameObject HoverGO
        {
            get
            {
                return hoverGO;
            }
            set
            {
                hoverGO = value;
            }
        }
        [SerializeField] private GameObject hoverGO = default;

        public ButtonType BtnType
        {
            get
            {
                return btnType;
            }
            set
            {
                btnType = value;
            }
        }
        [SerializeField] private ButtonType btnType = default;

        public int BtnNumber
        {
            get
            {
                return btnNumber;
            }
            set
            {
                btnNumber = value;
            }
        }
        [SerializeField] private int btnNumber = -1;

        private readonly int uiLayer = 5;

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
        [SerializeField] private bool pointerOverUI = default;


        private void Reset()
        {
            if (mainCam == null)
            {
                mainCam = FindObjectOfType<Camera>();
            }
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

        public bool GetButton(ButtonType type, int index)
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
            Vector2 worldPos = MousePositionToWorld();
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        public static Vector2 MousePositionToWorld()
        {
            return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.transform.position.z));
        }

        //public bool IsOverUI()
        //{
        //    if (HoverGO == null)
        //    {
        //        return false;
        //    }
        //    if (HoverGO.layer == uiLayer)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //Returns 'true' if we touched or hovering on Unity UI element.
        private bool IsPointerOverUI()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }


        //Returns 'true' if we touched or hovering on Unity UI element.
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
        {
            for (int index = 0; index < eventSystemRaycastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaycastResults[index];
                if (curRaysastResult.gameObject.layer == uiLayer)
                    return true;
            }
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

        //public static T DetectClick<T>(int btn, ButtonType type = ButtonType.Down) where T : MonoBehaviour
        //{
        //    if (!CheckButton(btn, type))
        //    {
        //        return null;
        //    }
        //    Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        //    if (hit.collider != null && hit.collider.gameObject.TryGetComponent<T>(out T mono))
        //    {
        //        return mono;
        //    }
        //    return null;
        //}
        //public static bool HitCollider(int btn, ButtonType type = ButtonType.Down)
        //{
        //    if (!CheckButton(btn, type))
        //    {
        //        return false;
        //    }
        //    Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        //    return hit.collider != null;
        //}
        //public static Vector2 MouseToWorldPos()
        //{
        //    return Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        //}
        //public static GameObject DetectHover()
        //{
        //    Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        //    if (hit.collider != null)
        //    {
        //        return hit.collider.gameObject;
        //    }
        //    return null;
        //}
        //public static T DetectHover<T>() where T : MonoBehaviour
        //{
        //    Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        //    if (hit.collider != null && hit.collider.gameObject.TryGetComponent<T>(out T mono))
        //    {
        //        return mono;
        //    }
        //    return null;
        //}
    }
}