using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.HotkeySystem
{
    public class HotkeyButton : HotkeyUser
    {
        [SerializeField] private Button btn = default;

        private void Reset()
        {
            btn = GetComponent<Button>();
        }

        protected override void OnHotkeyPressed()
        {
            btn.onClick?.Invoke();
        }
    }
}