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