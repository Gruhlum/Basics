using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Provides utility methods for file and directory operations, including
    /// reading, writing, serialization, and Unity-specific asset loading.
    /// </summary>
    public static class FileManager
    {
        private static string baseDirectory;

        /// <summary>
        /// Gets the base directory inside the user's Documents folder,
        /// named after the current Unity product.
        /// </summary>
        public static string BaseDirectory
        {
            get
            {
                baseDirectory ??= Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Application.productName);
                return baseDirectory;
            }
        }

        /// <summary>
        /// Deletes a folder and all its contents if it exists.
        /// </summary>
        public static void DeleteFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.Log($"Trying to delete folder, but not found: {path}");
                return;
            }

            Debug.Log($"Deleting folder: {path}");
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Returns a list of directory names inside the given path.
        /// </summary>
        public static List<string> GetDirectoryNames(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogWarning($"Directory does not exist: {path}");
                return null;
            }

            return GetPathEndObjects(Directory.GetDirectories(path));
        }


        /// <summary>
        /// Deletes a file if it exists.
        /// </summary>
        public static void DeleteFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log($"Trying to delete file, but not found: {path}");
                return;
            }

            Debug.Log($"Deleting file: {path}");
            File.Delete(path);
        }

        /// <summary>
        /// Returns a list of file names inside the given directory.
        /// </summary>
        public static List<string> GetFileNames(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.Log($"Directory does not exist: {path}");
                return null;
            }

            return GetPathEndObjects(Directory.GetFiles(path));
        }

        /// <summary>
        /// Reads all files in a directory and returns their contents as lists of strings.
        /// </summary>
        public static List<List<string>> ReadAllFiles(string path)
        {
            string[] filePaths = Directory.GetFiles(path);
            List<List<string>> results = new List<List<string>>();

            foreach (string filePath in filePaths)
            {
                results.Add(ReadFile(filePath));
            }

            return results;
        }

        /// <summary>
        /// Reads a file and returns its lines as a list of strings.
        /// </summary>
        public static List<string> ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"File does not exist: {path}");
                return null;
            }

            List<string> text = new List<string>();

            using StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                text.Add(line);
            }

            return text;
        }

        /// <summary>
        /// Reads a file inside a directory by name.
        /// </summary>
        public static List<string> ReadFile(string directoryPath, string fileName)
        {
            return ReadFile(Path.Combine(directoryPath, fileName));
        }
        /// <summary>
        /// Writes a list of strings to a text file.
        /// </summary>
        public static void WriteToFile(string path, string fileName, List<string> text)
        {
            Directory.CreateDirectory(path);

            using StreamWriter sw = new StreamWriter(Path.Combine(path, fileName));
            foreach (string line in text)
            {
                sw.WriteLine(line);
            }
        }
        public static void WriteToFile(string path, string fileName, string text)
        {
            Directory.CreateDirectory(path);
            string filePath = Path.Combine(path, fileName);
            using StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(text);
        }
        /// <summary>
        /// Writes raw bytes to a file.
        /// </summary>
        public static void WriteBytes(byte[] data, string path, string name, string fileEnding)
        {
            File.WriteAllBytes(Path.Combine(path, name + fileEnding), data);
        }
        /// <summary>
        /// Checks whether a file exists in a directory.
        /// </summary>
        public static bool FileExists(string directory, string fileName)
        {
            string filePath = Path.Combine(directory, fileName);
            Debug.Log(filePath);
            return File.Exists(filePath);
        }


        /// <summary>
        /// Extracts the final segment (file or folder name) from a path.
        /// </summary>
        public static string GetEndOfPathName(string path)
        {
            int startIndex = path.LastIndexOf("\\", StringComparison.Ordinal) + 1;
            return path.Substring(startIndex, path.Length - startIndex);
        }
        /// <summary>
        /// Converts an array of paths into a list of file or folder names.
        /// </summary>
        public static List<string> GetPathEndObjects(string[] paths)
        {
            if (paths == null)
            {
                return null;
            }

            List<string> fileNames = new List<string>();
            foreach (string result in paths)
            {
                fileNames.Add(GetEndOfPathName(result));
            }

            return fileNames;
        }
        /// <summary>
        /// Returns all file names inside a directory.
        /// </summary>
        public static List<string> FindAllFiles(string directoryPath)
        {
            return GetFileNames(directoryPath);
        }
        /// <summary>
        /// Generates a unique file name by appending an incrementing suffix.
        /// </summary>
        public static string GenerateUniqueFileName(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }

            string fileName = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.Log($"Could not find file name: {path}");
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


        /// <summary>
        /// Saves an object as a JSON file.
        /// </summary>
        public static void SaveJSON(object obj, string directoryPath, string fileName, bool prettyPrint = false)
        {
            string filePath = Path.Combine(directoryPath, fileName);

            try
            {
                Directory.CreateDirectory(directoryPath);

                using StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.Create));
                sw.WriteLine(JsonUtility.ToJson(obj, prettyPrint));
            }
            catch (Exception)
            {
                Debug.Log($"Could not save file, path: {filePath}");
            }
        }
        /// <summary>
        /// Loads a JSON file and deserializes it into the specified type.
        /// </summary>
        public static T LoadJSON<T>(string directory, string fileName) where T : class
        {
            string path = Path.Combine(directory, fileName);

            try
            {
                if (!File.Exists(path))
                {
                    return default;
                }

                using StreamReader sr = new StreamReader(path);
                return JsonUtility.FromJson<T>(sr.ReadToEnd());
            }
            catch (Exception)
            {
                Debug.Log($"Could not load file, path: {path}");
                return null;
            }
        }
        /// <summary>
        /// Loads all JSON files in a directory into a list of objects.
        /// </summary>
        public static List<T> LoadJSONAll<T>(string directoryPath) where T : class
        {
            List<string> results = FindAllFiles(directoryPath);
            List<T> items = new List<T>();

            if (results == null)
            {
                return items;
            }

            foreach (string result in results)
            {
                items.Add(LoadJSON<T>(directoryPath, GetEndOfPathName(result)));
            }

            return items;
        }


        /// <summary>
        /// Saves an object as an XML file.
        /// </summary>
        public static void SaveXML(object obj, string directoryPath, string fileName)
        {
            string filePath = Path.Combine(directoryPath, fileName);
            Directory.CreateDirectory(directoryPath);

            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using FileStream stream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(stream, obj);
        }

        /// <summary>
        /// Loads an XML file and deserializes it into the specified type.
        /// </summary>
        public static T LoadXML<T>(string directoryPath, string fileName) where T : class
        {
            string filePath = Path.Combine(directoryPath, fileName);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"{filePath} does not exist");
                return default;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using FileStream stream = new FileStream(filePath, FileMode.Open);
            return serializer.Deserialize(stream) as T;
        }

        /// <summary>
        /// Loads all XML files in a directory into a list of objects.
        /// </summary>
        public static List<T> LoadXMLAll<T>(string directoryPath) where T : class
        {
            List<string> results = FindAllFiles(directoryPath);
            List<T> items = new List<T>();

            if (results == null)
            {
                return items;
            }

            foreach (string result in results)
            {
                items.Add(LoadXML<T>(directoryPath, GetEndOfPathName(result)));
            }

            return items;
        }


        /// <summary>
        /// Loads a sprite from disk using a file name and directory.
        /// </summary>
        public static Sprite LoadSprite(string name, string directoryPath, string fileEnding = ".png")
        {
            string filePath = Path.Combine(directoryPath, name + fileEnding);

            if (!File.Exists(filePath))
            {
                Debug.Log($"File does not exist: {filePath}");
                return null;
            }

            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D tex2D = new Texture2D(1, 1);
            tex2D.LoadImage(bytes);

            return Sprite.Create(
                tex2D,
                new Rect(0, 0, tex2D.width, tex2D.height),
                Vector2.zero);
        }
    }
}