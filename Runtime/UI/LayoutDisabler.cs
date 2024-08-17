using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class LayoutDisabler : MonoBehaviour
    {
        [SerializeField] private HorizontalOrVerticalLayoutGroup layoutGroup = default;
        [SerializeField] private ContentSizeFitter contentSizeFitter = default;


        private void Reset()
        {
            layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            contentSizeFitter = GetComponent<ContentSizeFitter>();
        }

        private IEnumerator Start()
        {
            yield return null;
            if (contentSizeFitter != null)
            {
                contentSizeFitter.enabled = false;
            }
            if (layoutGroup != null)
            {
                layoutGroup.enabled = false;
            }
        }
    }
}