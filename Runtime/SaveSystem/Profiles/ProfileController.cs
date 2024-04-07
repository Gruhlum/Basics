using HexTecGames.Basics.UI.Displays;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.Profiles
{
    public class ProfileController : MonoBehaviour
    {
        [SerializeField] private ProfileDisplayController profileDisplayC = default;
        [SerializeField] private InputDisplay inputDisplay = default;
        [SerializeField] private ConfirmDisplay deleteConfirm = default;
        [SerializeField] private TMP_Text currentProfileNameGUI = default;

        private Profile selectedProfile;

        private void Awake()
        {           
            inputDisplay.OnInputConfirmed += InputDisplay_OnInputConfirmed;
            deleteConfirm.OnCancelClicked += DeleteConfirm_OnCancelClicked;
            deleteConfirm.OnConfirmClicked += DeleteConfirm_OnConfirmClicked;
        }
        private void OnDestroy()
        {
            inputDisplay.OnInputConfirmed -= InputDisplay_OnInputConfirmed;
            deleteConfirm.OnCancelClicked -= DeleteConfirm_OnCancelClicked;
            deleteConfirm.OnConfirmClicked -= DeleteConfirm_OnConfirmClicked;
        }
        private void Start()
        {
            DisplayProfiles();
            if (profileDisplayC.GetTotalItems() == 0)
            {
                inputDisplay.Show("Create Profile");
            }
        }

        private void DeleteConfirm_OnConfirmClicked()
        {
            SaveSystem.RemoveProfile(selectedProfile);
            selectedProfile = null;
            UpdateCurrentProfileDisplay();
            DisplayProfiles();
        }

        private void UpdateCurrentProfileDisplay()
        {
            if (SaveSystem.CurrentProfile == null)
            {
                currentProfileNameGUI.text = null;
            }
            else currentProfileNameGUI.text = SaveSystem.CurrentProfile.Name;
        }

        private void DeleteConfirm_OnCancelClicked()
        {
            selectedProfile = null;
        }    

        private void InputDisplay_OnInputConfirmed(string input)
        {
            if (selectedProfile == null)
            {
                AddProfile(input);
            }
            else
            {
                SaveSystem.RenameProfile(selectedProfile, input);
                selectedProfile = null;
                DisplayProfiles();
            }           
            UpdateCurrentProfileDisplay();
        }

        private void DisplayProfiles()
        {
            var results = SaveSystem.GetProfiles();
            profileDisplayC.SetItems(results);
            UpdateCurrentProfileDisplay();
        }

        public void RenameProfile(Profile profile)
        {
            selectedProfile = profile;
            inputDisplay.Show("Rename", profile.Name);          
        }
        private string GenerateText(string itemName)
        {
            return $"Permanently delete '{itemName}'?";
        }
        public void SelectProfile(Profile profile)
        {
            SaveSystem.SetProfile(profile);
            currentProfileNameGUI.text = profile.Name;
        }
        public void DeleteProfile(Profile profile)
        {
            selectedProfile = profile;
            deleteConfirm.Setup(GenerateText(profile.Name));
        }
        public void AddProfileClicked()
        {
            inputDisplay.Show("Create Profile");
        }

        private void AddProfile(string name)
        {
            SaveSystem.AddProfile(name);
            DisplayProfiles();
        }
    }
}