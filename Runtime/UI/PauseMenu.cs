using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class PauseMenu : MenuController
    {
        [SerializeField] protected GameObject menuGO = default;

        public event Action<bool> OnMenuToggled;

        public PermissionGroup AllowToggle = new PermissionGroup();


        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        public void ToggleMenu()
        {
            ActivateMenu(!menuGO.activeInHierarchy);
        }
        public void ActivateMenu(bool active)
        {
            if (!AllowToggle.Allowed)
            {
                return;
            }
            ActivateMenuEffect(active);
        }
        protected virtual void ActivateMenuEffect(bool active)
        {
            menuGO.SetActive(active);
            OnMenuToggled?.Invoke(active);
        }
    }
}