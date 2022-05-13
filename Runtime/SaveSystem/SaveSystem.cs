using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Basics
{
	public static class SaveSystem
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

        public static void SaveJSON(object obj, string fileName, string directory = "data", bool prettyPrint = false)
        {
            CheckDirectories(directory);
            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(BaseDirectory, directory, fileName), FileMode.Create)))
            {
                sw.WriteLine(JsonUtility.ToJson(obj, prettyPrint));
            }
        }
        public static T LoadJSON<T>(string fileName, string directory = "data") where T : class
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
        public static void SaveXML(object obj, string fileName, string directory = "data")
        {
            CheckDirectories(directory);
            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new FileStream(Path.Combine(BaseDirectory, directory, fileName), FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }
        public static T LoadXML<T>(string fileName, string directory = "data") where T : class
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
    }
}