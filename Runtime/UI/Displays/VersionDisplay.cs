using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class VersionDisplay : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI versionGUI = default;

        public void Awake()
        {
            versionGUI.text = Application.version;
        }
    }
}