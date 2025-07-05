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
            int firstCharIndex = textInfo.linkInfo[index].linkTextfirstCharacterIndex;
            int lastCharIndex = textInfo.linkInfo[index].linkTextLength + firstCharIndex - 1;

            TMP_CharacterInfo firstCharInfo = textInfo.characterInfo[firstCharIndex];
            TMP_CharacterInfo lastCharInfo = textInfo.characterInfo[lastCharIndex];

            Vector3 btmLeftWorld = textGUI.transform.TransformPoint(firstCharInfo.bottomLeft);
            Vector3 btmRightWorld = textGUI.transform.TransformPoint(lastCharInfo.bottomRight);

            btmLeftWorld.y -= heightOffset;
            btmRightWorld.y -= heightOffset;

            Vector3 btmLeftScreen = Camera.main.WorldToScreenPoint(btmLeftWorld);
            Vector3 btmRightScreen = Camera.main.WorldToScreenPoint(btmRightWorld);

            Debug.Log(textGUI.text + " - " + firstCharInfo.character + " - " + lastCharInfo.character + " - " + btmLeftWorld + " - " + btmRightWorld, textGUI);

            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = btmRightScreen.x - btmLeftScreen.x;
            sizeDelta.x = (Mathf.Floor(sizeDelta.x / 20) * 20) + 10;
            rectTransform.sizeDelta = sizeDelta;
            Rect rect = rectTransform.rect;
            rectTransform.position = (btmRightWorld + btmLeftWorld) / 2f;
        }

    }
}