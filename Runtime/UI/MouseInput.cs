using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public static class MouseInput
	{
        public enum ButtonType { None, Down, Clicked, Up }
        private static bool CheckButton(int btn, ButtonType type)
        {
            if (type == ButtonType.Down && !Input.GetMouseButtonDown(btn))
            {
                return false;
            }
            if (type == ButtonType.Clicked && !Input.GetMouseButton(btn))
            {
                return false;
            }
            if (type == ButtonType.Up && !Input.GetMouseButtonUp(btn))
            {
                return false;
            }
            return true;
        }
        public static GameObject DetectClick(int btn, ButtonType type = ButtonType.Down)
        {
            if (!CheckButton(btn, type))
            {
                return null;
            }
            Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            return null;
        }
        public static T DetectClick<T>(int btn, ButtonType type = ButtonType.Down) where T : MonoBehaviour
        {
            if (!CheckButton(btn, type))
            {
                return null;
            }
            Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent<T>(out T mono))
            {
                return mono;
            }
            return null;
        }
        public static bool HitCollider(int btn, ButtonType type = ButtonType.Down)
        {
            if (!CheckButton(btn, type))
            {
                return false;
            }
            Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            return hit.collider != null;
        }
        public static Vector2 MouseToWorldPos()
        {
            return Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        }
        public static GameObject DetectHover()
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            return null;
        }
        public static T DetectHover<T>() where T : MonoBehaviour
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent<T>(out T mono))
            {
                return mono;
            }
            return null;
        }
    }
}