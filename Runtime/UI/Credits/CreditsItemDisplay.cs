using HexTecGames.Basics.UI;
using HexTecGames.Basics.UI.Buttons;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.Credits
{
    public class CreditsItemDisplay : Display<CreditsItemDisplay, CreditsItem>
    {
        [SerializeField] private TMP_Text roleGUI = default;
        [SerializeField] private TMP_Text nameGUI = default;
        [SerializeField] private LinkButton linkButton = default;


        protected override void DrawItem(CreditsItem item)
        {
            nameGUI.text = item.name;
            roleGUI.text = item.role;
            if (linkButton != null)
            {
                linkButton.URL = item.linkURL;
            }
            name = $"CreditsDisplay ({item.name})";
        }
    }
}