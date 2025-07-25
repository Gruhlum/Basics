using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Allows moving and zooming of a Camera
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam = default;

        public float ZoomSpeed = 1;
        public float MinZoom = 5;
        public float MaxZoom = 50;

        public float MoveStep = 1f;

        // Used to prevent 'snapping' the zoom when the applcation becomes focussed again
        private bool ignoreInput;

        private void Reset()
        {
            cam = GetComponent<Camera>();
        }
        private void Update()
        {
            if (!Application.isFocused)
            {
                ignoreInput = true;
                return;
            }

            if (ignoreInput)
            {
                ignoreInput = false;
                return;
            }
            HandlePositionMovement();
            if (!MouseController.IsPointerOverUI)
            {
                HandleZoom();
            }
        }

        private void HandleZoom()
        {
            cam.orthographicSize -= Input.mouseScrollDelta.normalized.y * ZoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, MinZoom, MaxZoom);
        }

        public void SetPosition(Vector2 position)
        {
            cam.transform.position = new Vector3(position.x, position.y, cam.transform.position.z);
        }

        private void HandlePositionMovement()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0f, MoveStep * Time.deltaTime, 0f);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-MoveStep * Time.deltaTime, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += new Vector3(0f, -MoveStep * Time.deltaTime, 0f);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(MoveStep * Time.deltaTime, 0f, 0f);
            }
        }
    }
}