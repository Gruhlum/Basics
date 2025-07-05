using System.Collections.Generic;
using HexTecGames.Basics.UI;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.Profiles
{
    public class ProfileController : MonoBehaviour
    {
        [SerializeField] private ProfileDisplayController profileDisplayC = default;
        [SerializeField] private InputWindow inputWindow = default;
        [SerializeField] private ConfirmWindow deleteConfirm = default;
        [SerializeField] private TMP_Text currentProfileNameGUI = default;

        private Profile selectedProfile;

        private void Awake()
        {
            inputWindow.OnInputConfirmed += InputDisplay_OnInputConfirmed;
            Debug.LogError("Not working correctly!");
            // Changed events from C# to Unity for ConfirmWindow
        }
        private void OnDestroy()
        {
            inputWindow.OnInputConfirmed -= InputDisplay_OnInputConfirmed;
        }
        private void Start()
        {
            DisplayProfiles();
            if (profileDisplayC.TotalItems == 0)
            {
                inputWindow.Show("Create Profile");
            }
        }

        public void ConfirmClicked()
        {
            SaveSystem.RemoveProfile(selectedProfile);
            selectedProfile = null;
            UpdateCurrentProfileDisplay();
            DisplayProfiles();
        }
        public void CancelClicked()
        {
            selectedProfile = null;
        }
        private void UpdateCurrentProfileDisplay()
        {
            if (SaveSystem.CurrentProfile == null)
            {
                currentProfileNameGUI.text = null;
            }
            else currentProfileNameGUI.text = SaveSystem.CurrentProfile.Name;
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
            List<Profile> results = SaveSystem.GetProfiles();
            profileDisplayC.SetItems(results);
            UpdateCurrentProfileDisplay();
        }

        public void RenameProfile(Profile profile)
        {
            selectedProfile = profile;
            inputWindow.Show("Rename", profile.Name);
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
            inputWindow.Show("Create Profile");
        }

        private void AddProfile(string name)
        {
            SaveSystem.AddProfile(name);
            DisplayProfiles();
        }
    }
}