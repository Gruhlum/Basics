using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor.BuildHelper
{
	[System.Serializable]
	public class StoreSettings
	{
		public bool activate;
		public string name;
        [Tooltip("GameObjects that will only be included when this is Setting is active")]
        public List<GameObject> specificPrefabs;

        [Tooltip("Used to copy the build folders to another location")]
        public List<CopyFolder> copyFolders;

        [Tooltip("Location of the external script")]
        public string externalScript;
        [Tooltip("Should this script be run after Build is complete")]
        public bool runExternalScript;

	}

}