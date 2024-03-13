using HexTecGames.Basics.Profiles;
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
        private static readonly string profileKey = "profile";

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

        public static string GetFullBaseDirectory
        {
            get
            {
                if (CurrentProfile != null)
                {
                    return Path.Combine(BaseDirectory, CurrentProfile.Name);
                }
                else return BaseDirectory;
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
                SaveSettings(profileKey, null, false);
                return;
            }
            var result = profiles.Find(x => x == profile);
            if (result == null)
            {
                Debug.Log("Could not find profile with name " + profile.Name);
                return;
            }
            SaveSettings(profileKey, result.Name, false);
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
            if (CurrentProfile == profile)
            {
                SaveSettings(profileKey, profile.Name, false);
            }
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
            var results =  FileManager.GetDirectoryNames(BaseDirectory);
            profiles.Clear();
            foreach (var result in results)
            {
                if (result == defaultFolderName)
                {
                    continue;
                }
                profiles.Add(new Profile(result));
            }
            loadedProfiles = true;

            SetProfile(profiles.Find(x => x.Name == LoadSettings(profileKey)));
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

        public static void SaveSettings(string key, string value, bool isProfile = true)
        {
            SettingsData data = LoadSettingsData();
            if (data == null)
            {
                data = new SettingsData();
            }
            data.SetOption(key, value);
            SaveSettingsData(data, isProfile);
        }
        private static void SaveSettingsData(SettingsData data, bool isProfile)
        {
            if (isProfile)
            {
                SaveJSON(data, settingsFileName, Path.Combine(GetFullBaseDirectory, defaultFolderName));
            }
            else SaveJSON(data, settingsFileName, Path.Combine(BaseDirectory, defaultFolderName));
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
            SettingsData data = LoadJSON<SettingsData>(settingsFileName, Path.Combine(GetFullBaseDirectory, defaultFolderName));
            return data;
        }
        private static void CheckDirectories(string directory)
        {
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }
            if (CurrentProfile != null && !Directory.Exists(CurrentProfile.Name))
            {
                Directory.CreateDirectory(CurrentProfile.Name);
            }
            if (!Directory.Exists(Path.Combine(GetFullBaseDirectory, directory)))
            {
                Directory.CreateDirectory(Path.Combine(GetFullBaseDirectory, directory));
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
            try
            {
                CheckDirectories(directory);
                using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(GetFullBaseDirectory, directory, fileName), FileMode.Create)))
                {
                    sw.WriteLine(JsonUtility.ToJson(obj, prettyPrint));
                }
            }
            catch (Exception)
            {

                Debug.Log("Could not save file");
            }

        }
        public static T LoadJSON<T>(string fileName) where T : class
        {
            return LoadJSON<T>(fileName,Path.Combine(GetFullBaseDirectory, defaultFolderName));
        }
        public static T LoadJSON<T>(string fileName, string directory) where T : class
        {
            try
            {
                string path = Path.Combine(GetFullBaseDirectory, directory, fileName);

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
                Debug.Log("Could not load file");
                return null;
            }
        }
        public static List<T> LoadJSONAll<T>() where T : class
        {
            return LoadJSONAll<T>(GetFullBaseDirectory);
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
            using (var stream = new FileStream(Path.Combine(GetFullBaseDirectory, directory, fileName), FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }
        public static T LoadXML<T>(string fileName) where T : class
        {
            return LoadXML<T>(fileName, Path.Combine(GetFullBaseDirectory, defaultFolderName));
        }
        public static T LoadXML<T>(string fileName, string directory) where T : class
        {
            string path = Path.Combine(GetFullBaseDirectory, directory, fileName);

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
            return LoadXMLAll<T>(GetFullBaseDirectory);
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