﻿using HexTecGames.Basics.Profiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace HexTecGames.Basics
{
    public static class SaveSystem
    {
        private static readonly string defaultFolderName = "Data";
        private static readonly string settingsFileName = "Settings.txt";
        private static readonly string defaultProfileName = "Profile 1";
        private static readonly string backupFolderName = "backup";
        //private static readonly string profileKey = "profile";

        private static SettingsData settingsData;

        private static List<Profile> profiles = new List<Profile>();
        private static bool loadedProfiles;

        public static Profile CurrentProfile
        {
            get
            {
                if (!loadedProfiles)
                {
                    LoadProfiles();
                }
                return currentProfile;
            }
            private set
            {
                currentProfile = value;
            }
        }
        private static Profile currentProfile;

        /// <summary>
        /// The base folder path. Usually inside a folder named after the product in the user's Document folder.
        /// <code>
        /// if (Application.platform == RuntimePlatform.WebGLPlayer)
        /// {
        ///     return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.productName)
        /// }
        /// else return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.productName);
        /// </code>
        /// </summary>
        public static string BaseDirectory
        {
            get
            {
                if (baseDirectory == null)
                {
                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        baseDirectory = Path.Combine("idbfs", Application.productName + "_" + Application.companyName);
                    }
                    else baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.productName);
                }
                return baseDirectory;
            }
        }
        private static string baseDirectory;

        /// <summary>
        /// The BaseDirectory + the current profile folder.
        /// </summary>
        public static string ProfilePath
        {
            get
            {
                if (CurrentProfile != null)
                {
                    return Path.Combine(BaseDirectory, CurrentProfile.Name);
                }
                else return Path.Combine(BaseDirectory, defaultProfileName);
            }
        }
        public static void SetProfile(Profile profile)
        {
            if (!loadedProfiles)
            {
                LoadProfiles();
            }
            if (profile == null)
            {
                CurrentProfile = null;
                //SaveSettings(profileKey, null);
                return;
            }
            var result = profiles.Find(x => x == profile);
            if (result == null)
            {
                Debug.Log("Could not find profile with name " + profile.Name);
                return;
            }
            //SaveSettings(profileKey, result.Name);
            CurrentProfile = result;
        }
        public static void AddProfile(string name, bool select = true)
        {
            if (!loadedProfiles)
            {
                LoadProfiles();
            }
            name.RemoveInvalidSymbols();
            name = name.GetUniqueName(profiles.Select(x => x.Name));
            Profile profile = new Profile(name);
            profiles.Add(profile);
            Directory.CreateDirectory(Path.Combine(BaseDirectory, name));
            if (CurrentProfile == null || select)
            {
                SetProfile(profile);
            }
        }
        public static void RenameProfile(Profile profile, string name)
        {
            name.RemoveInvalidSymbols();
            List<Profile> allProfiles = new List<Profile>();
            allProfiles.AddRange(profiles);
            allProfiles.Remove(profile);
            name = name.GetUniqueName(allProfiles.Select(x => x.Name));
            if (profile.Name == name)
            {
                return;
            }
            var oldName = profile.Name;
            profile.Rename(name);
            Directory.Move(Path.Combine(BaseDirectory, oldName), Path.Combine(BaseDirectory, name));
            //if (CurrentProfile == profile)
            //{
            //    SaveSettings(profileKey, profile.Name);
            //}
        }
        public static void RemoveProfile(Profile profile)
        {
            if (!loadedProfiles)
            {
                LoadProfiles();
            }
            var result = profiles.Find(x => x == profile);
            if (result == null)
            {
                Debug.Log("Could not find profile with name " + profile.Name);
                return;
            }
            profiles.Remove(result);

            Directory.Delete(Path.Combine(BaseDirectory, result.Name), true);

            if (CurrentProfile == result)
            {
                SetProfile(null);
            }
        }
        private static void LoadProfiles()
        {
            loadedProfiles = true;
            Debug.Log("Loading Profiles");
            var results = FileManager.GetDirectoryNames(BaseDirectory);
            profiles.Clear();
            if (results == null)
            {
                Debug.Log("No Profile found, adding default profile");
                AddProfile(defaultProfileName, true);
                return;
            }
            foreach (var result in results)
            {
                if (result == defaultFolderName)
                {
                    continue;
                }
                profiles.Add(new Profile(result));
            }

            //SetProfile(profiles.Find(x => x.Name == LoadSettings(profileKey)));
        }
        public static List<Profile> GetProfiles()
        {
            if (!loadedProfiles)
            {
                LoadProfiles();
            }
            List<Profile> results = new List<Profile>();
            results.AddRange(profiles);
            return results;
        }

        /// <summary>
        /// Saves a value under a specified key inside a JSON file. 
        /// </summary>
        /// <see cref="BaseDirectory"/>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveSettings(string key, string value)
        {
            if (settingsData == null)
            {
                settingsData = LoadSettingsData();
            }
            settingsData.SetOption(key, value);
            SaveJSON(settingsData, settingsFileName);
        }
        public static void SaveSettings(string key, bool value)
        {
            if (settingsData == null)
            {
                settingsData = LoadSettingsData();
            }
            settingsData.SetOption(key, value.ToString());
            SaveJSON(settingsData, settingsFileName);
        }
        public static void SaveSettings(string key, int value)
        {
            if (settingsData == null)
            {
                settingsData = LoadSettingsData();
            }
            settingsData.SetOption(key, value.ToString());
            SaveJSON(settingsData, settingsFileName);
        }
        public static void SaveSettings(string key, float value)
        {
            if (settingsData == null)
            {
                settingsData = LoadSettingsData();
            }
            settingsData.SetOption(key, value.ToString());
            SaveJSON(settingsData, settingsFileName);
        }
        /// <summary>
        /// Retrieves a value assigned to the key.
        /// </summary>
        /// <param name="key">The key that this value is saved under.</param>
        /// <returns>A string value, or null if none could be found.</returns>
        public static string LoadSettings(string key)
        {
            if (settingsData == null)
            {
                settingsData = LoadSettingsData();
            }

            return settingsData.GetOption(key);
        }
        public static bool LoadSettings(string key, ref int value)
        {
            string result = LoadSettings(key);
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            try
            {
                value = Convert.ToInt32(result);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Key: " + key + " Error: " + e.Message);
                return false;
            }
        }
        public static bool LoadSettings(string key, ref float value)
        {
            string result = LoadSettings(key);
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            try
            {
                value = Convert.ToInt32(result);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Key: " + key + " Error: " + e.Message);
                return false;
            }
        }
        public static bool LoadSettings(string key, ref bool value)
        {
            string result = LoadSettings(key);
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            try
            {
                value = Convert.ToBoolean(result);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Key: " + key + " Error: "  + e.Message);
                value = false;
                return false;
            }
        }
        private static SettingsData LoadSettingsData()
        {
            SettingsData data = LoadJSON<SettingsData>(settingsFileName);
            if (data == null)
            {
                return new SettingsData();
            }
            else return data;
        }
        private static void CheckDirectories(string directory)
        {
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }
            if (CurrentProfile != null && !Directory.Exists(ProfilePath))
            {
                Directory.CreateDirectory(ProfilePath);
            }
            if (!Directory.Exists(Path.Combine(ProfilePath, directory)))
            {
                Directory.CreateDirectory(Path.Combine(ProfilePath, directory));
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
        /// <summary>
        /// Saves an object as a JSON file inside the BaseDirectory folder.
        /// </summary>
        /// <see cref="BaseDirectory"/>
        /// <param name="obj">The object that will be saved.</param>
        /// <param name="fileName">The name the file will have.</param>
        /// <param name="prettyPrint">Should the JSON file have "prettyPrint" or not.</param>
        public static void SaveJSON(object obj, string fileName, bool prettyPrint = false)
        {
            SaveJSON(obj, fileName, defaultFolderName, prettyPrint);
        }
        /// <summary>
        /// Saves an object as a JSON file inside the BaseDirectory folder.
        /// </summary>
        /// <see cref="BaseDirectory"/>
        /// <param name="obj">The object that will be saved.</param>
        /// <param name="fileName">The name the file will have.</param>
        /// <param name="directory">The name of the subfolder.</param>
        /// <param name="prettyPrint">Should the JSON file have "prettyPrint" or not.</param>
        public static void SaveJSON(object obj, string fileName, string directory, bool prettyPrint = false)
        {
            string path = Path.Combine(ProfilePath, directory, fileName);
            try
            {
                CheckDirectories(directory);
                Debug.Log($"Saving {obj} as JSON file to: {path}");
                using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create)))
                {
                    sw.WriteLine(JsonUtility.ToJson(obj, prettyPrint));
                }
            }
            catch (Exception)
            {
                Debug.Log($"Could not save file, path: {path}");
            }
        }
        /// <summary>
        /// Tries to load a JSON file with a specified name and returns it as a specified type.
        /// </summary>
        /// <typeparam name="T">The type that the object will be returned as.</typeparam>
        /// <param name="fileName">The name of the file that should be loaded.</param>
        /// <returns>The loaded object, or null if it could not be found.</returns>
        public static T LoadJSON<T>(string fileName) where T : class
        {
            return LoadJSON<T>(fileName, defaultFolderName);
        }
        /// <summary>
        /// Tries to load a JSON file with a specified name and returns it as a specified type.
        /// </summary>
        /// <typeparam name="T">The type that the object will be returned as.</typeparam>
        /// <param name="fileName">The name of the file that should be loaded.</param>
        /// <param name="directory">The name of the Folder containing the JSON file.</param>
        /// <returns>The loaded object, or null if it could not be found.</returns>
        public static T LoadJSON<T>(string fileName, string directory) where T : class
        {
            string path = Path.Combine(ProfilePath, directory, fileName);
            try
            {
                if (!File.Exists(path))
                {
                    //Debug.LogWarning(path + " does not exist");
                    return default;
                }
                using (StreamReader sr = new StreamReader(path))
                {
                    return JsonUtility.FromJson<T>(sr.ReadToEnd());
                }
            }
            catch (Exception)
            {
                Debug.Log($"Could not load file, path: {path}");
                return null;
            }
        }
        /// <summary>
        /// Tries to load any JSON files that are inside the specified folder.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="directory">The name of the folder containing the JSON Files</param>
        /// <returns>The loaded objects, or an empty list of none could be found.</returns>
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
        /// <summary>
        /// Saves an object as an XML file inside the BaseDirectory folder.
        /// </summary>
        /// <see cref="BaseDirectory"/>
        /// <param name="obj">The object that will be saved.</param>
        /// <param name="fileName">The name the file will have.</param>
        public static void SaveXML(object obj, string fileName)
        {
            SaveXML(obj, fileName, defaultFolderName);
        }
        /// <summary>
        /// Saves an object as an XML file inside the BaseDirectory folder.
        /// </summary>
        /// <see cref="BaseDirectory"/>
        /// <param name="obj">The object that will be saved.</param>
        /// <param name="fileName">The name the file will have.</param>
        /// <param name="directory">The name of the subfolder.</param>
        public static void SaveXML(object obj, string fileName, string directory)
        {
            CheckDirectories(directory);
            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new FileStream(Path.Combine(ProfilePath, directory, fileName), FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }
        /// <summary>
        /// Tries to load an XML file with a specified name and returns it as a specified type.
        /// </summary>
        /// <typeparam name="T">The type that the object will be returned as.</typeparam>
        /// <param name="fileName">The name of the file that should be loaded.</param>
        /// <returns>The loaded object, or null if it could not be found.</returns>
        public static T LoadXML<T>(string fileName) where T : class
        {
            return LoadXML<T>(fileName, defaultFolderName);
        }
        /// <summary>
        /// Tries to load an XML file with a specified name and returns it as a specified type.
        /// </summary>
        /// <typeparam name="T">The type that the object will be returned as.</typeparam>
        /// <param name="fileName">The name of the file that should be loaded.</param>
        /// <param name="directory">The name of the Folder containing the JSON file.</param>
        /// <returns>The loaded object, or null if it could not be found.</returns>
        public static T LoadXML<T>(string fileName, string directory) where T : class
        {
            string path = Path.Combine(ProfilePath, directory, fileName);

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
        /// <summary>
        /// Tries to load any XML files that are inside the specified folder.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="directory">The name of the folder containing the JSON Files</param>
        /// <returns>The loaded objects, or an empty list of none could be found.</returns>
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