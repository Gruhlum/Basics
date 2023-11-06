using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class FileManager
    {
        public static string BaseDirectory
        {
            get
            {
                if (baseDirectory == null)
                {
                    baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.productName);
                }
                return baseDirectory;
            }
        }
        private static string baseDirectory;

        public static void VerifyDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        private static void VerifyFile(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
        }

        public static string[] GetFilePaths(string folderName)
        {
            var dirPath = Path.Combine(BaseDirectory, folderName);

            if (!Directory.Exists(dirPath))
            {
                Debug.LogWarning("Directory does not exist: " + dirPath);
                return null;
            }

            return Directory.GetFiles(dirPath);
        }
        public static List<string> GetFileNames(string folderName)
        {
            var results = GetFilePaths(folderName);
            if (results == null)
            {
                return null;
            }
            List<string> fileNames = new List<string>();
            foreach (var result in results)
            {
                int startIndex = result.LastIndexOf(@"\") + 1;
                int length = result.Length - startIndex;
                fileNames.Add(result.Substring(startIndex, length));
            }
            return fileNames;
        }
        public static List<List<string>> ReadMultipleFiles(string folderName)
        {
            string[] filePaths = GetFilePaths(folderName);

            List<List<string>> results = new List<List<string>>();

            foreach (var filePath in filePaths)
            {
                results.Add(ReadFile(filePath));
            }

            return results;
        }
        public static string GetUniqueFileName(string path, string name)
        {
            string fullPath = Path.Combine(path, name);
            if (!Directory.Exists(fullPath))
            {
                return name;
            }
            int count = 2;
            while (Directory.Exists(Path.Combine($"{fullPath} {count}")))
            {
                count++;
            }
            return $"{fullPath} {count}";
        }
        public static List<string> ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("File does not exist: " + filePath);
                return null;
            }

            List<string> text = new List<string>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    text.Add(line);
                }
            }
            return text;
        }

        public static void WriteBytes(byte[] data, string path, string name, string fileEnding)
        {
            File.WriteAllBytes(Path.Combine(path, name + fileEnding), data);
        }
        public static Sprite LoadSprite(string path, string name, string fileEnding = ".png")
        {
            string fullPath = Path.Combine(path, name + fileEnding);
            if (!File.Exists(fullPath))
            {
                Debug.Log("File does not exists: " + fullPath);
                return null;
            }
            byte[] bytes = File.ReadAllBytes(fullPath);
            Texture2D tex2D = new Texture2D(1, 1);
            tex2D.LoadImage(bytes);
            return Sprite.Create(tex2D, new Rect(new Vector2(0, 0), new Vector2(tex2D.width, tex2D.height)), Vector2.zero);
        }

        public static List<string> ReadFile(string fileName, string subFolderName)
        {
            var filePath = Path.Combine(BaseDirectory, subFolderName, fileName, ".txt");

            return ReadFile(filePath);
        }
        public static string GetSubFolderPath(string subfolderName)
        {
            return Path.Combine(BaseDirectory, subfolderName);
        }
        public static void WriteToFile(string fileName, string filePath, List<string> text)
        {
            VerifyDirectory(BaseDirectory);
            VerifyDirectory(filePath);

            //VerifyFile(Path.Combine(filePath, fileName + ".txt"));
            using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, fileName + ".txt")))
            {
                foreach (var line in text)
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}