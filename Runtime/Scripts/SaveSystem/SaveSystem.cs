using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace HexTecGames.Basics
{
    public static class SaveSystem
    {
        private static readonly string defaultFolderName = "Data";
        private static readonly string settingsFileName = "Settings.txt";

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

        public static void SaveSettings(string key, string value)
        {
            SettingsData data = LoadSettingsData();
            if (data == null)
            {
                data = new SettingsData();
            }
            data.SetOption(key, value);
            SaveSettingsData(data);
        }
        private static void SaveSettingsData(SettingsData data)
        {
            SaveJSON(data, settingsFileName, Path.Combine(BaseDirectory, defaultFolderName));
        }
        public static string LoadSettings(string key)
        {
            SettingsData data = LoadSettingsData();
            if (data == null)
            {
                return null;
            }
            return data.GetOption(key);
        }
        private static SettingsData LoadSettingsData()
        {
            SettingsData data = LoadJSON<SettingsData>(settingsFileName, Path.Combine(BaseDirectory, defaultFolderName));
            return data;
        }
        private static void CheckDirectories(string directory)
        {
            if (Directory.Exists(BaseDirectory) == false)
            {
                Directory.CreateDirectory(BaseDirectory);
            }
            if (Directory.Exists(Path.Combine(BaseDirectory, directory)) == false)
            {
                Directory.CreateDirectory(Path.Combine(BaseDirectory, directory));
            }
        }
        public static List<string> FindAllFiles()
        {
            return FindAllFiles(defaultFolderName);
        }
        public static List<string> FindAllFiles(string directory)
        {
            return FileManager.GetFileNames(directory);
        }
        public static void SaveJSON(object obj, string fileName, bool prettyPrint = false)
        {
            SaveJSON(obj, fileName, defaultFolderName, prettyPrint);
        }
        public static void SaveJSON(object obj, string fileName, string directory, bool prettyPrint = false)
        {
            CheckDirectories(directory);
            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(BaseDirectory, directory, fileName), FileMode.Create)))
            {
                sw.WriteLine(JsonUtility.ToJson(obj, prettyPrint));
            }
        }
        public static T LoadJSON<T>(string fileName) where T : class
        {
            return LoadJSON<T>(fileName, defaultFolderName);
        }
        public static T LoadJSON<T>(string fileName, string directory) where T : class
        {
            string path = Path.Combine(BaseDirectory, directory, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning(path + " does not exist");
                return default;
            }
            using (StreamReader sr = new StreamReader(path))
            {
                return JsonUtility.FromJson<T>(sr.ReadToEnd());
            }
        }
        public static List<T> LoadJSONAll<T>() where T : class
        {
            return LoadJSONAll<T>(BaseDirectory);
        }
        public static List<T> LoadJSONAll<T>(string directory) where T : class
        {
            var results = FindAllFiles(directory);
            List<T> levels = new List<T>();
            foreach (var result in results)
            {
                levels.Add(LoadJSON<T>(result, directory));
            }
            return levels;
        }
        public static void SaveXML(object obj, string fileName)
        {
            SaveXML(obj, fileName, defaultFolderName);
        }
        public static void SaveXML(object obj, string fileName, string directory)
        {
            CheckDirectories(directory);
            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new FileStream(Path.Combine(BaseDirectory, directory, fileName), FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }
        public static T LoadXML<T>(string fileName) where T : class
        {
            return LoadXML<T>(fileName, defaultFolderName);
        }
        public static T LoadXML<T>(string fileName, string directory) where T : class
        {
            string path = Path.Combine(BaseDirectory, directory, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning(path + " does not exist");
                return default;
            }
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as T;
            }
        }
        public static List<T> LoadXMLAll<T>() where T : class
        {
            return LoadXMLAll<T>(BaseDirectory);
        }
        public static List<T> LoadXMLAll<T>(string directory) where T : class
        {
            var results = FindAllFiles(directory);
            if (results == null)
            {
                return null;
            }
            List<T> levels = new List<T>();
            foreach (var result in results)
            {
                levels.Add(LoadXML<T>(result, directory));
            }
            return levels;
        }
    }
}