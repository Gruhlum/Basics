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
            ToggleMenu(!menuGO.activeInHierarchy);
        }
        public virtual void ToggleMenu(bool active)
        {
            if (!AllowToggle.Allowed)
            {
                return;
            }
            menuGO.SetActive(active);
            OnMenuToggled?.Invoke(active);
        }
    }
}