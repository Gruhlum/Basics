using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class ScreenshotController : MonoBehaviour
	{
        [SerializeField] private Camera cam = default;


        private void Awake()
        {
            Debug.Log(Application.dataPath);
        }

        [ContextMenu("Take Screenshot")]
        public void TakeScreenshot()
        {
            TakeScreenshot(Application.dataPath, "screenie.png");
        }
        public void TakeScreenshot(string path, string name)
        {
            byte[] result = GenerateScreenshot();
            SaveScreenshot(result, path, name);
        }
        public byte[] GenerateScreenshot()
        {
            RenderTexture screenTexture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, -1);
            cam.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            cam.Render();
            Texture2D renderedTexture = new Texture2D(cam.pixelWidth, cam.pixelHeight);
            renderedTexture.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);
            RenderTexture.active = null;
            return renderedTexture.EncodeToPNG();
        }
        public void SaveScreenshot(byte[] data, string path, string name, string fileEnding = ".png")
        {
            FileManager.WriteBytes(data, path, name, fileEnding);
        }
    }
}