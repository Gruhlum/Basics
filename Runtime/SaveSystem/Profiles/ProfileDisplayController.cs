using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.Profiles
{
    public class ProfileDisplayController : DisplayController<Profile>
    {
        [SerializeField] private ProfileController profileC = default;



        protected override void DisplayItems()
        {
            base.DisplayItems();
            HighlightActiveProfile();
        }

        private void HighlightActiveProfile()
        {
            foreach (var display in displaySpawner.GetActiveInstances())
            {
                if (display is ProfileDisplay profileDisplay)
                {
                    if (SaveSystem.CurrentProfile == null || profileDisplay.Item == null)
                    {
                        profileDisplay.SetHighlighted(false);
                    }
                    else profileDisplay.SetHighlighted(SaveSystem.CurrentProfile.Name == profileDisplay.Item.Name);
                }
            }
        }

        public override void DisplayClicked(Display<Profile> display)
        {
            base.DisplayClicked(display);
            profileC.SelectProfile(display.Item);
            HighlightActiveProfile();
        }

        public void DeleteClicked(ProfileDisplay display)
        {
            profileC.DeleteProfile(display.Item);
        }
        public void RenameClicked(ProfileDisplay display)
        {
            profileC.RenameProfile(display.Item);
        }
    }
}