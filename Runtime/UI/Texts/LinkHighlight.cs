using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class LinkHighlight : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform = default;
        [Space]
        [SerializeField] private float heightOffset = 0.06f;

        public void Setup(TMP_Text textGUI, TMP_TextInfo textInfo, int index)
        {
            var firstCharIndex = textInfo.linkInfo[index].linkTextfirstCharacterIndex;
            var lastCharIndex = textInfo.linkInfo[index].linkTextLength + firstCharIndex - 1;

            var firstCharInfo = textInfo.characterInfo[firstCharIndex];
            var lastCharInfo = textInfo.characterInfo[lastCharIndex];

            var btmLeftWorld = textGUI.transform.TransformPoint(firstCharInfo.bottomLeft);
            var btmRightWorld = textGUI.transform.TransformPoint(lastCharInfo.bottomRight);

            btmLeftWorld.y -= heightOffset;
            btmRightWorld.y -= heightOffset;

            var btmLeftScreen = Camera.main.WorldToScreenPoint(btmLeftWorld);
            var btmRightScreen = Camera.main.WorldToScreenPoint(btmRightWorld);

            Debug.Log(textGUI.text + " - " + firstCharInfo.character + " - " + lastCharInfo.character + " - " + btmLeftWorld + " - " + btmRightWorld, textGUI);

            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = (btmRightScreen.x - btmLeftScreen.x);
            sizeDelta.x = (Mathf.Floor(sizeDelta.x / 20) * 20) + 10;
            rectTransform.sizeDelta = sizeDelta;
            var rect = rectTransform.rect;
            rectTransform.position = (btmRightWorld + btmLeftWorld) / 2f;
        }

    }
}