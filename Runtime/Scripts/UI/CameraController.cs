using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera cam = default;

        public int ScrollSpeed = 10;
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.position += new Vector3(0f, MoveStep, 0f);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position += new Vector3(-MoveStep, 0f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.position += new Vector3(0f, -MoveStep, 0f);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += new Vector3(MoveStep, 0f, 0f);
            }

            float scrollDelta = Input.mouseScrollDelta.y;
            cam.orthographicSize -= scrollDelta * ZoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, MinZoom, MaxZoom);
        }
    }
}