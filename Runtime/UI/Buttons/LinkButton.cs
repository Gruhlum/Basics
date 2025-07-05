using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    public class LinkButton : BaseButton
    {
        public string URL
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        [SerializeField][TextArea] private string url = default;

        protected override void ClickEffect()
        {
            Application.OpenURL(url);
        }
    }
}