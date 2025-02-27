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

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static List<string> GetPathEndObjects(string[] paths)
        {
            if (paths == null)
            {
                return null;
            }
            List<string> fileNames = new List<string>();
            foreach (var result in paths)
            {
                fileNames.Add(GetEndOfPathName(result));
            }
            return fileNames;
        }
        public static string GetEndOfPathName(string path)
        {
            int startIndex = path.LastIndexOf(@"\") + 1;
            int length = path.Length - startIndex;
            return path.Substring(startIndex, length);
        }

        public static void DeleteFile(string fileName, string path)
        {
            string finalPath = Path.Combine(path, fileName);
            if (!File.Exists(finalPath))
            {
                Debug.Log("Trying to delete file, but not found: " + finalPath);
                return;
            }
            Debug.Log("Deleting file: " + finalPath);
            File.Delete(finalPath);
        }
        public static void DeleteFile(string fileName)
        {
            DeleteFile(fileName, BaseDirectory);
        }
        public static void DeleteFolder(string folderName, string path)
        {
            string finalPath = Path.Combine(path, folderName);
            if (!Directory.Exists(finalPath))
            {
                Debug.Log("Trying to delete folder, but not found: " + finalPath);
                return;
            }
            Debug.Log("Deleting folder: " + finalPath);
            Directory.Delete(finalPath, true);
        }
        public static void DeleteFolder(string folderName)
        {
            DeleteFolder(folderName, BaseDirectory);
        }
        public static List<string> GetFileNames(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.Log("Directory does not exist: " + path);
                return null;
            }
            var results = Directory.GetFiles(path);
            return GetPathEndObjects(results);
        }
        public static List<string> GetDirectoryNames(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogWarning("Directory does not exist: " + path);
                return null;
            }
            return GetPathEndObjects(Directory.GetDirectories(path));
        }
        public static List<List<string>> ReadAllFiles(string path)
        {
            string[] filePaths = Directory.GetFiles(path);

            List<List<string>> results = new List<List<string>>();

            foreach (var filePath in filePaths)
            {
                results.Add(ReadFile(filePath));
            }

            return results;
        }
        /// <summary>
        /// Generates a unique file name.
        /// </summary>
        /// <param name="path">the original path of the file.</param>
        /// <returns>returns the original path unless it is not unique, in which case a underscore and number suffix is added.</returns>
        public static string GenerateUniqueFileName(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.Log("Could not find file name: " + path);
                return path;
            }
            int count = 1;
            string newPath;
            do
            {
                count++;
                newPath = path.Replace(fileName, $"{fileName}_{count}");
            }
            while (File.Exists(newPath));
            return newPath;
        }
        //public static string GetUniqueFileName(string path, string name)
        //{
        //    string fullPath = Path.Combine(path, name);
        //    if (!Directory.Exists(fullPath))
        //    {
        //        return name;
        //    }
        //    int count = 2;
        //    while (Directory.Exists(Path.Combine($"{fullPath} {count}")))
        //    {
        //        count++;
        //    }
        //    return $"{fullPath} {count}";
        //}
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
            CreateDirectory(BaseDirectory);
            CreateDirectory(filePath);

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