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

        public void SetBlockState(object sender, bool allow)
        {
            if (allow)
            {
                blockers.Remove(sender);
            }
            else blockers.Add(sender);

            Allowed = blockers.Count <= 0;
        }
    }
}