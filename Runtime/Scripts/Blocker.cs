using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class Blocker
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

        private readonly List<object> blockers = new List<object>();

        public event Action<bool> OnAllowedChanged;

        public void SetAllowState(object sender, bool allow)
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

            UpdateBlockState();
        }
        private void UpdateBlockState()
        {
            Allowed = blockers.Count <= 0;
        }
        public void ClearAll()
        {
            blockers.Clear();
            UpdateBlockState();
        }
    }
}