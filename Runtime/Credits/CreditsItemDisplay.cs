using HexTecGames.Basics.UI;
using HexTecGames.Basics.UI.Buttons;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.Credits
{
    public class CreditsItemDisplay : Display<CreditsItemDisplay, CreditsItem>
    {
        [SerializeField] private TMP_Text nameGUI = default;
        [SerializeField] private TMP_Text roleGUI = default;
        [SerializeField] private LinkButton linkButton = default;


        protected void Reset()
        {
            linkButton = GetComponent<LinkButton>();
        }

        protected override void DrawItem(CreditsItem item)
        {
            nameGUI.text = item.name;
            roleGUI.text = item.role;
            linkButton.URL = item.itchURL;
            name = $"CreditsDisplay ({item.name})";
        }
    }
}