using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera cam = default;

        public int ScrollSpeed = 10;
        public int ZoomSpeed = 10;
        public int MinZoom = 5;
        public int MaxZoom = 40;

        private void Reset()
        {
            cam = Camera.main;
        }
        private void Update()
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            cam.orthographicSize -= scrollDelta * ZoomSpeed * Time.deltaTime;
        }
    }
}