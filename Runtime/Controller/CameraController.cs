using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	/// <summary>
    /// Allows moving and zooming of a Camera
    /// </summary>
    public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera cam = default;

        public int ZoomSpeed = 10;
        public int MinZoom = 5;
        public int MaxZoom = 40;

        public float MoveStep = 1f;

        private void Reset()
        {
            cam = GetComponent<Camera>();
        }
        private void Update()
        {
            if (!Application.isFocused)
            {
                return;
            }
            HandlePositionMovement();
            HandleZoom();
        }

        private void HandleZoom()
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            cam.orthographicSize -= scrollDelta * ZoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, MinZoom, MaxZoom);
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