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

        public override void OnHotkeyPressed()
        {
            btn.onClick?.Invoke();
        }
    }
}