using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class PermissionGroup
    {
        public bool Allowed
        {
            get
            {
                return allowed;
            }
            private set
            {
                if (allowed == value)
                {
                    return;
                }
                allowed = value;
                OnAllowedChanged?.Invoke(allowed);
            }
        }
        [SerializeField] private bool allowed = true;

        private readonly List<Component> blockers = new List<Component>();

        public event Action<bool> OnAllowedChanged;

        public void SetPermissionState(Component sender, bool allow)
        {
            if (allow)
            {
                for (int i = blockers.Count - 1; i >= 0; i--)
                {
                    if (blockers[i] == sender)
                    {
                        blockers.RemoveAt(i);
                    }
                }
            }
            else blockers.Add(sender);

            UpdatePermissionState();
        }
        private void UpdatePermissionState()
        {
            Allowed = blockers.Count <= 0;
        }
        public void ClearAll()
        {
            blockers.Clear();
            UpdatePermissionState();
        }
    }
}