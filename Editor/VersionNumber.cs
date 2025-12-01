using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    public class VersionNumber
    {
        private int Major;
        private int Medium;
        private int Minor;

        public VersionNumber(int major, int medium, int minor)
        {
            this.Major = major;
            this.Medium = medium;
            this.Minor = minor;
        }
        public VersionNumber(string version)
        {
            var versionParts = version.Split('.');

            if (versionParts.Length != 3)
            {
                Debug.Log("Invalid version format. Expected format: Major.Medium.Minor");
                return;
            }

            int.TryParse(versionParts[0], out Major);
            int.TryParse(versionParts[1], out Medium);
            int.TryParse(versionParts[2], out Minor);
        }

        //private static VersionNumber CreateVersionNumber()
        //{
        //    string result = GetCurrentVersion();
        //    return new VersionNumber(result);
        //}
        //public static void SetVersionNumber(string versionNumber)
        //{
        //    PlayerSettings.bundleVersion = versionNumber;
        //}
        //public static void SetVersionNumber(VersionNumber versionNumber)
        //{
        //    PlayerSettings.bundleVersion = versionNumber.ToString();
        //}
        public static VersionNumber GetVersionNumber()
        {
            return new VersionNumber(PlayerSettings.bundleVersion);
        }
        public static VersionNumber GetVersionNumber(UpdateType updateType)
        {
            VersionNumber number = new VersionNumber(PlayerSettings.bundleVersion);
            number.IncreaseVersion(updateType);
            return number;
        }
        //public static void IncreaseVersion(UpdateType updateType)
        //{
        //    VersionNumber number = CreateVersionNumber();
        //    number.ApplyIncrease(updateType);
        //    number.SaveToBuild();
        //}
        public static void SetBuildVersionNumber(VersionNumber versionNumber)
        {
            PlayerSettings.bundleVersion = versionNumber.ToString();
        }

        public VersionNumber GetIncreasedVersion(UpdateType updateType)
        {
            VersionNumber result = new VersionNumber(Major, Medium, Minor);
            result.IncreaseVersion(updateType);
            return result;
        }

        public void IncreaseVersion(UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.Major:
                    Major++;
                    Medium = 0;
                    Minor = 0;
                    break;

                case UpdateType.Medium:
                    Medium++;
                    Minor = 0;
                    break;

                case UpdateType.Minor:
                    Minor++;
                    break;

                case UpdateType.None:
                default:
                    break;
            }
        }
        public override string ToString()
        {
            return $"{Major}.{Medium}.{Minor}";
        }

    }
}