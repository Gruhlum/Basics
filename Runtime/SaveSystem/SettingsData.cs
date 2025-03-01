using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class SettingsData
    {
        public List<Setting> Values = new List<Setting>();

        public void SetOption(string key, string value)
        {
            Setting setting = Values.Find(x => x.Key == key);
            if (setting != null)
            {
                setting.Value = value;
            }
            else Values.Add(new Setting(key, value));
        }
        public string GetOption(string key)
        {
            Setting setting = Values.Find(x => x.Key == key);
            if (setting != null)
            {
                return setting.Value;
            }
            return null;
        }

        public void DeleteOption(string key)
        {
            var result = Values.Find(x => x.Key == key);
            if (result != null)
            {
                Values.Remove(result);
            }
        }

        [System.Serializable]
        public class Setting
        {
            public string Key;
            public string Value;

            public Setting(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}