using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics
{
    public static class VersionData
    {
        public static string CurrentVersion
        {
            get
            {
                return MajorVersion.ToString() + MediumVersion.ToString() + MinorVersion.ToString();
            }
        }
        public static int MinorVersion = 0;
        public static int MediumVersion = 0;
        public static int MajorVersion = 1;
        public enum UpdateType { None, Minor, Medium, Major }
        public enum VersionType { Full, Demo }
        public static VersionType CurrentVersionType;

        public static void IncreaseVersion(UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.None:
                    return;
                case UpdateType.Minor:
                    MinorVersion++;
                    break;
                case UpdateType.Medium:
                    MediumVersion++;
                    break;
                case UpdateType.Major:
                    MajorVersion++;
                    break;
                default:
                    break;
            }
        }
#if UNITY_EDITOR
        public static void SetVersion()
        {
            PlayerSettings.bundleVersion = CurrentVersion;
        }
        public static void GetVersion()
        {
            string version = PlayerSettings.bundleVersion;
            string[] splitVersion = version.Split('.');
            if (splitVersion.Length < 2)
            {
                return;
            }
            MajorVersion = Convert.ToInt16(splitVersion[0]);
            MediumVersion = Convert.ToInt16(splitVersion[1]);
            MinorVersion = Convert.ToInt16(splitVersion[2]);
        }
#endif
    }
}