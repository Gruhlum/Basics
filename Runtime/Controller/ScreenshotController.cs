using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HexTecGames.Basics
{
	/// <summary>
    /// Class that allows you to take and save screenshots from the camera's data.
    /// </summary>
    public class ScreenshotController : MonoBehaviour
	{
        [SerializeField] private Camera cam = default;

        [SerializeField, TextArea] private string defaultSavePath = Application.dataPath;
        [SerializeField] private string defaultScreenshotName = "screenshot";


        /// <summary>
        /// Generates and saves a screenshot.
        /// </summary>
        [ContextMenu("Take Screenshot")]
        public void TakeScreenshot()
        {
            TakeScreenshot(defaultSavePath, defaultScreenshotName + ".png");
        }

        /// <summary>
        /// Generates and saves a screenshot.
        /// </summary>
        /// <param name="path">Absolute path where the screenshot will be saved to.</param>
        /// <param name="name">Name of the file, should include a file ending (like .png).</param>
        public void TakeScreenshot(string path, string name)
        {
            TakeScreenshot(path, name, cam);
        }

        /// <summary>
        /// Generates and saves a screenshot.
        /// </summary>
        /// <param name="path">Absolute path where the screenshot will be saved to.</param>
        /// <param name="name">Name of the file, should include a file ending (like .png).</param>
        /// <param name="cam">Camera that will be used to generate the screenshot.</param>
        public void TakeScreenshot(string path, string name, Camera cam)
        {
            byte[] result = GenerateScreenshot(cam);
            SaveScreenshot(result, path, name);
        }
       
        private byte[] GenerateScreenshot(Camera cam)
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
        private void SaveScreenshot(byte[] data, string path, string name, string fileEnding = ".png")
        {
            FileManager.WriteBytes(data, path, name, fileEnding);
        }
    }
}