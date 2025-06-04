using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class LinkHighlighter : MonoBehaviour
    {
        [SerializeField] private Spawner<LinkHighlight> highlightSpawner = default;

        public void Setup(TMP_Text textGUI)
        {
            highlightSpawner.DeactivateAll();

            var textInfo = textGUI.GetTextInfo(textGUI.text);

            for (int i = 0; i < textInfo.linkCount; i++)
            {
                highlightSpawner.Spawn().Setup(textGUI, textInfo, i);
            }
        }
    }
}