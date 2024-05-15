using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera cam = default;

        //public int ScrollSpeed = 10;
        public int ZoomSpeed = 10;
        public int MinZoom = 5;
        public int MaxZoom = 40;

        public float MoveStep = 1f;

        private void Reset()
        {
            cam = Camera.main;
        }
        private void Update()
        {
            if (!Application.isFocused)
            {
                return;
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0f, MoveStep * Time.deltaTime, 0f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-MoveStep * Time.deltaTime, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0f, -MoveStep * Time.deltaTime, 0f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(MoveStep * Time.deltaTime, 0f, 0f);
            }

            float scrollDelta = Input.mouseScrollDelta.y;
            cam.orthographicSize -= scrollDelta * ZoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, MinZoom, MaxZoom);
        }
    }
}