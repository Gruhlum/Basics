using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class UIMenuController : MonoBehaviour
    {
        [SerializeField] private List<SubMenu> subMenus = default;

        [SerializeField] private KeyCode returnKey = KeyCode.Escape;

        public PermissionGroup AllowOpening = new PermissionGroup();

        private SubMenu currentMenu;

        public event Action<SubMenu> OnActiveMenuChanged;

        private void Awake()
        {
            if (subMenus == null || subMenus.Count == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            if (subMenus[0].gameObject.activeInHierarchy)
            {
                SetCurrentMenu(subMenus[0]);
            }
        }

        private void Update()
        {
            if (!AllowOpening.Allowed)
            {
                return;
            }
            if (Input.GetKeyDown(returnKey))
            {
                ReturnKeyPressed();
            }
        }

        private void ReturnKeyPressed()
        {
            ReturnPreviousMenu();
        }

        public void ReturnPreviousMenu()
        {
            if (currentMenu == null)
            {
                SetCurrentMenu(subMenus[0]);
            }
            else SetCurrentMenu(currentMenu.PreviousMenu);
        }

        public void SetCurrentMenu(SubMenu menu)
        {
            if (currentMenu != null)
            {
                currentMenu.SetActive(false);
            }

            currentMenu = menu;

            if (currentMenu != null)
            {
                currentMenu.SetActive(true);
            }

            OnActiveMenuChanged?.Invoke(currentMenu);
        }
    }
}