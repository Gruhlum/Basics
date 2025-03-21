using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.Profiles
{
    public class ProfileDisplayController : DisplayController<ProfileDisplay, Profile>
    {
        [SerializeField] private ProfileController profileController = default;

        public override void DisplayItems()
        {
            base.DisplayItems();
            HighlightActiveProfile();
        }

        public override void SubscribeEvents(ProfileDisplay display)
        {
            base.SubscribeEvents(display);
            display.OnRenameClicked += Display_OnRenameClicked;
            display.OnDeleteClicked += Display_OnDeleteClicked;
        }
        public override void UnsubscribeEvents(ProfileDisplay display)
        {
            base.UnsubscribeEvents(display);
            display.OnRenameClicked -= Display_OnRenameClicked;
            display.OnDeleteClicked -= Display_OnDeleteClicked;
        }

        private void Display_OnDeleteClicked(ProfileDisplay display)
        {
            profileController.DeleteProfile(display.Item);
        }

        private void Display_OnRenameClicked(ProfileDisplay display)
        {
            profileController.RenameProfile(display.Item);
        }

        private void HighlightActiveProfile()
        {
            foreach (var display in displaySpawner)
            {
                if (!display.gameObject.activeSelf)
                {
                    continue;
                }
                if (SaveSystem.CurrentProfile == null || display.Item == null)
                {
                    display.SetHighlighted(false);
                }
                else display.SetHighlighted(SaveSystem.CurrentProfile.Name == display.Item.Name);
            }
        }

        protected override void Display_OnDisplayClicked(ProfileDisplay display)
        {
            base.Display_OnDisplayClicked(display);
            profileController.SelectProfile(display.Item);
            HighlightActiveProfile();
        }
    }
}