using System;
using System.Collections.Generic;
using System.Diagnostics;
using Object = System.Object;

namespace HexTecGames.Basics
{
    /// <summary>
    /// A permission gate that becomes disallowed when one or more senders block it. 
    /// </summary>
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
                allowed = value;
            }
        }
        private bool allowed = true;

        public int BlockersCount => blockers.Count;

        private readonly HashSet<Object> blockers = new HashSet<Object>();

        public event Action<bool> OnAllowedChanged;

        public bool HasBlocker(object sender) => blockers.Contains(sender);
        public void SetPermissionState(object sender, bool allow)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            if (allow)
            {
                blockers.Remove(sender);
            }
            else blockers.Add(sender);

            UpdatePermissionState();
        }
        private void UpdatePermissionState()
        {
            bool newState = blockers.Count == 0;
            if (newState != Allowed)
            {
                Allowed = newState;
                OnAllowedChanged?.Invoke(Allowed);
            }
        }
        public void ClearAll()
        {
            blockers.Clear();
            UpdatePermissionState();
        }

        public override string ToString()
        {
            return $"Allowed: {Allowed}, Blockers: {blockers.Count}";
        }
    }
}