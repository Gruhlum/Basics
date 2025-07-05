using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class IconDisplay : Display<IconDisplay, IconData>
    {
        [SerializeField] private Image img = default;

        protected override void DrawItem(IconData item)
        {
            img.sprite = item.sprite;
        }
    }
}