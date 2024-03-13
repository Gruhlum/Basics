using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexTecGames.Basics.Profiles
{
    public class ProfileDisplay : Display<Profile>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text nameGUI = default;
        [SerializeField] private GameObject buttonsGO = default;

        [SerializeField] private Image backgroundImg = default;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightedColor = Color.white;


        private void OnDisable()
        {
            ShowButtons(false);
        }

        public void SetHighlighted(bool highlighted)
        {
            backgroundImg.color = highlighted ? highlightedColor : normalColor;
        }

        protected override void DrawItem(Profile profile)
        {
            if (profile == null)
            {
                nameGUI.text = null;
            }
            else nameGUI.text = profile.Name;
        }

        public void ShowButtons(bool show)
        {
            if (string.IsNullOrEmpty(nameGUI.text))
            {
                buttonsGO.SetActive(false);
                return;
            }
            buttonsGO.SetActive(show);
        }

        public void DeleteClicked()
        {
            if (displayC is ProfileDisplayController profileDisplayC)
            {
                profileDisplayC.DeleteClicked(this);
            }
        }
        public void RenameClicked()
        {
            if (displayC is ProfileDisplayController profileDisplayC)
            {
                profileDisplayC.RenameClicked(this);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowButtons(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ShowButtons(false);
        }
    }
}