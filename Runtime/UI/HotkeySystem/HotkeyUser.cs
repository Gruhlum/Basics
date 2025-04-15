using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HexTecGames.HotkeySystem
{
    public abstract class HotkeyUser : MonoBehaviour
    {
        [SerializeField] private KeyCode keyCode = default;

        public KeyCode KeyCode
        {
            get
            {
                return this.keyCode;
            }
            set
            {
                this.keyCode = value;
            }
        }

        public abstract void OnHotkeyPressed();
    }
}