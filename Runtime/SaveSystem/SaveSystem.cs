using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using HexTecGames.Basics.Profiles;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Centralized system for managing profiles, settings, and save data.
    /// Handles JSON/XML serialization, profile switching, and directory routing.
    /// </summary>
    public static class SaveSystem
    {
        private static readonly string defaultFolderName = "Data";
        private static readonly string settingsFileName = "Settings.txt";
        private static readonly string defaultProfileName = "Profile 1";
        private static readonly string backupFolderName = "backup";

        private static SettingsData settingsData;
        private static List<Profile> profiles = new List<Profile>();
        private static bool loadedProfiles;
        private static Profile currentProfile;


        /// <summary>
        /// The base directory where all profiles and data are stored.
        /// On WebGL, this uses IDBFS. On other platforms, it uses the user's Documents folder.
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
                    else
                    {
                        baseDirectory = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                            Application.productName);
                    }
                }
                return baseDirectory;
            }
        }
        private static string baseDirectory;

        /// <summary>
        /// The directory for the currently active profile.
        /// If no profile is selected, the default profile name is used.
        /// </summary>
        public static string ProfilePath =>
            CurrentProfile != null
                ? Path.Combine(BaseDirectory, CurrentProfile.Name)
                : Path.Combine(BaseDirectory, defaultProfileName);

        /// <summary>
        /// The directory where general data files are stored for the current profile.
        /// </summary>
        public static string DataDirectory => Path.Combine(ProfilePath, defaultFolderName);



        /// <summary>
        /// The currently active profile. Automatically loads profiles if needed.
        /// </summary>
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
            private set => currentProfile = value;
        }

        /// <summary>
        /// Loads all profile folders from disk.
        /// Creates a default profile if none exist.
        /// </summary>
        private static void LoadProfiles()
        {
            loadedProfiles = true;
            profiles.Clear();

            List<string> results = FileManager.GetDirectoryNames(BaseDirectory);

            if (results == null)
            {
                Debug.Log("No profiles found, creating default profile.");
                AddProfile(defaultProfileName, true);
                return;
            }

            foreach (string result in results)
            {
                if (result == defaultFolderName)
                    continue;

                profiles.Add(new Profile(result));
            }
        }

        /// <summary>
        /// Returns a copy of all available profiles.
        /// </summary>
        public static List<Profile> GetProfiles()
        {
            if (!loadedProfiles)
            {
                LoadProfiles();
            }

            return new List<Profile>(profiles);
        }

        /// <summary>
        /// Sets the active profile if it exists.
        /// </summary>
        public static void SetProfile(Profile profile)
        {
            if (!loadedProfiles)
            {
                LoadProfiles();
            }

            if (profile == null)
            {
                CurrentProfile = null;
                return;
            }

            Profile result = profiles.Find(x => x == profile);
            if (result == null)
            {
                Debug.Log($"Could not find profile with name {profile.Name}");
                return;
            }

            CurrentProfile = result;
        }

        /// <summary>
        /// Creates a new profile with a unique name and optionally selects it.
        /// </summary>
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

        /// <summary>
        /// Renames an existing profile and moves its directory.
        /// </summary>
        public static void RenameProfile(Profile profile, string name)
        {
            name.RemoveInvalidSymbols();

            List<Profile> others = profiles.Where(x => x != profile).ToList();
            name = name.GetUniqueName(others.Select(x => x.Name));

            if (profile.Name == name)
                return;

            string oldName = profile.Name;
            profile.Rename(name);

            Directory.Move(Path.Combine(BaseDirectory, oldName), Path.Combine(BaseDirectory, name));
        }

        /// <summary>
        /// Removes a profile and deletes its directory.
        /// </summary>
        public static void RemoveProfile(Profile profile)
        {
            if (!loadedProfiles)
            {
                LoadProfiles();
            }

            Profile result = profiles.Find(x => x == profile);
            if (result == null)
            {
                Debug.Log($"Could not find profile with name {profile.Name}");
                return;
            }

            profiles.Remove(result);
            Directory.Delete(Path.Combine(BaseDirectory, result.Name), true);

            if (CurrentProfile == result)
            {
                SetProfile(null);
            }
        }


        private static void SaveSettingsToFile()
        {
            SaveJSON(settingsData, defaultFolderName, settingsFileName);
        }

        /// <summary>
        /// Saves a string setting value under the given key.
        /// </summary>
        public static void SaveSettings(string key, string value)
        {
            settingsData ??= LoadSettingsData();
            settingsData.SetOption(key, value);
            SaveSettingsToFile();
        }

        /// <summary>
        /// Saves a boolean setting value under the given key.
        /// </summary>
        public static void SaveSettings(string key, bool value)
        {
            settingsData ??= LoadSettingsData();
            settingsData.SetOption(key, value.ToString());
            SaveSettingsToFile();
        }

        /// <summary>
        /// Saves an integer setting value under the given key.
        /// </summary>
        public static void SaveSettings(string key, int value)
        {
            settingsData ??= LoadSettingsData();
            settingsData.SetOption(key, value.ToString());
            SaveSettingsToFile();
        }

        /// <summary>
        /// Saves a float setting value under the given key.
        /// </summary>
        public static void SaveSettings(string key, float value)
        {
            settingsData ??= LoadSettingsData();
            settingsData.SetOption(key, value.ToString());
            SaveSettingsToFile();
        }

        /// <summary>
        /// Attempts to load a string setting value.
        /// </summary>
        private static string LoadSettings(string key)
        {
            settingsData ??= LoadSettingsData();
            return settingsData.GetOption(key);
        }

        /// <summary>
        /// Attempts to load a string setting value with a default fallback.
        /// </summary>
        public static bool LoadSettings(string key, out string value, string defaultValue = null)
        {
            string result = LoadSettings(key);

            if (string.IsNullOrEmpty(result))
            {
                value = defaultValue;
                return false;
            }

            value = result;
            return true;
        }

        /// <summary>
        /// Attempts to load an integer setting value with a default fallback.
        /// </summary>
        public static bool LoadSettings(string key, out int value, int defaultValue = 0)
        {
            string result = LoadSettings(key);

            if (string.IsNullOrEmpty(result))
            {
                value = defaultValue;
                return false;
            }

            try
            {
                value = Convert.ToInt32(result);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Key: {key} Error: {e.Message}");
                value = defaultValue;
                return false;
            }
        }

        /// <summary>
        /// Attempts to load a float setting value with a default fallback.
        /// </summary>
        public static bool LoadSettings(string key, out float value, float defaultValue = 0)
        {
            string result = LoadSettings(key);

            if (string.IsNullOrEmpty(result))
            {
                value = defaultValue;
                return false;
            }

            try
            {
                value = (float)Convert.ToDouble(result);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Key: {key} Error: {e.Message}");
                value = defaultValue;
                return false;
            }
        }

        /// <summary>
        /// Attempts to load a boolean setting value with a default fallback.
        /// </summary>
        public static bool LoadSettings(string key, out bool value, bool defaultValue = false)
        {
            string result = LoadSettings(key);

            if (string.IsNullOrEmpty(result))
            {
                value = defaultValue;
                return false;
            }

            try
            {
                value = Convert.ToBoolean(result);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Key: {key} Error: {e.Message}");
                value = defaultValue;
                return false;
            }
        }

        /// <summary>
        /// Deletes a setting value by key.
        /// </summary>
        public static void DeleteSettings(string key)
        {
            settingsData ??= LoadSettingsData();
            settingsData.DeleteOption(key);
            SaveSettingsToFile();
        }

        /// <summary>
        /// Loads the settings file or creates a new one if missing.
        /// </summary>
        private static SettingsData LoadSettingsData()
        {
            SettingsData data = LoadJSON<SettingsData>(settingsFileName);
            return data ?? new SettingsData();
        }

#if UNITY_EDITOR
        [MenuItem("Tools/SaveSystem/Delete All Settings")]
#endif
        public static void DeleteAllSettings()
        {
            settingsData = new SettingsData();
            SaveSettingsToFile();
        }

#if UNITY_EDITOR
        [MenuItem("Tools/SaveSystem/Delete All Data")]
#endif
        public static void DeleteAllData()
        {
            FileManager.DeleteFolder(BaseDirectory);
        }


        /// <summary>
        /// Saves an object as JSON inside the default data folder.
        /// </summary>
        public static void SaveJSON(object obj, string fileName, bool prettyPrint = false)
        {
            SaveJSON(obj, defaultFolderName, fileName, prettyPrint);
        }

        /// <summary>
        /// Saves an object as JSON inside a specific subfolder of the current profile.
        /// </summary>
        public static void SaveJSON(object obj, string directoryName, string fileName, bool prettyPrint = false)
        {
            FileManager.SaveJSON(obj, GetProfileDirectoryPath(directoryName), fileName, prettyPrint);
        }

        /// <summary>
        /// Loads a JSON file from the default data folder.
        /// </summary>
        public static T LoadJSON<T>(string fileName) where T : class
        {
            return LoadJSON<T>(defaultFolderName, fileName);
        }

        /// <summary>
        /// Loads a JSON file from a specific subfolder of the current profile.
        /// </summary>
        public static T LoadJSON<T>(string directoryName, string fileName) where T : class
        {
            return FileManager.LoadJSON<T>(GetProfileDirectoryPath(directoryName), fileName);
        }

        /// <summary>
        /// Loads all JSON files inside a specific subfolder of the current profile.
        /// </summary>
        public static List<T> LoadJSONAll<T>(string directoryName) where T : class
        {
            return FileManager.LoadJSONAll<T>(GetProfileDirectoryPath(directoryName));
        }



        /// <summary>
        /// Saves an object as XML inside the default data folder.
        /// </summary>
        public static void SaveXML(object obj, string fileName)
        {
            SaveXML(obj, defaultFolderName, fileName);
        }

        /// <summary>
        /// Saves an object as XML inside a specific subfolder of the current profile.
        /// </summary>
        public static void SaveXML(object obj, string directoryName, string fileName)
        {
            string path = Path.Combine(ProfilePath, directoryName);
            FileManager.SaveXML(obj, path, fileName);
        }

        /// <summary>
        /// Loads an XML file from the default data folder.
        /// </summary>
        public static T LoadXML<T>(string fileName) where T : class
        {
            return LoadXML<T>(defaultFolderName, fileName);
        }

        /// <summary>
        /// Loads an XML file from a specific subfolder of the current profile.
        /// </summary>
        public static T LoadXML<T>(string directoryName, string fileName) where T : class
        {
            return FileManager.LoadXML<T>(Path.Combine(ProfilePath, directoryName), fileName);
        }

        /// <summary>
        /// Loads all XML files inside a specific subfolder of the current profile.
        /// </summary>
        public static List<T> LoadXMLAll<T>(string directory) where T : class
        {
            return FileManager.LoadXMLAll<T>(GetProfileDirectoryPath(directory));
        }



        /// <summary>
        /// Deletes a file inside the default data folder.
        /// </summary>
        public static void DeleteFile(string fileName)
        {
            DeleteFile(defaultFolderName, fileName);
        }

        /// <summary>
        /// Deletes a file inside a specific subfolder of the current profile.
        /// </summary>
        public static void DeleteFile(string directoryName, string fileName)
        {
            string path = Path.Combine(ProfilePath, directoryName, fileName);
            FileManager.DeleteFile(path);
        }

        /// <summary>
        /// Checks whether a file exists inside the default data folder.
        /// </summary>
        public static bool FileExists(string fileName)
        {
            return FileExists(defaultFolderName, fileName);
        }

        /// <summary>
        /// Checks whether a file exists inside a specific subfolder of the current profile.
        /// </summary>
        public static bool FileExists(string directory, string fileName)
        {
            return FileManager.FileExists(Path.Combine(ProfilePath, directory), fileName);
        }

        /// <summary>
        /// Returns the full path to a subfolder inside the current profile.
        /// </summary>
        private static string GetProfileDirectoryPath(string directoryName)
        {
            return Path.Combine(ProfilePath, directoryName);
        }
    }
}
