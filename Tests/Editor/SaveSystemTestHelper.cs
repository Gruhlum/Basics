using System.Collections.Generic;
using System.Reflection;
using HexTecGames.Basics.Profiles;

namespace HexTecGames.Basics.Tests
{
    static class SaveSystemTestHelper
    {
        public static void SetBaseDirectory(string path)
        {
            var field = typeof(SaveSystem).GetField("baseDirectory", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, path);
        }

        public static void ResetState()
        {
            // Reset static fields so tests don't bleed into each other
            typeof(SaveSystem).GetField("settingsData", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, null);

            typeof(SaveSystem).GetField("profiles", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, new List<Profile>());

            typeof(SaveSystem).GetField("loadedProfiles", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, false);

            typeof(SaveSystem).GetField("currentProfile", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, null);
        }
    }

}