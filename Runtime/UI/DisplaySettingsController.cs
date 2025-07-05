using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class DisplaySettingsController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resDropDown = default;
        [SerializeField] private TMP_Dropdown modeDropDown = default;

        [SerializeField] private float minimumResArea = 2000f;

        private void Start()
        {
            PopulateResolutionDropDown();
            TMP_Dropdown.OptionData option = modeDropDown.options.Find(x => x.text == Screen.fullScreenMode.ToString());
            int index = modeDropDown.options.IndexOf(option);
            if (index >= 0)
            {
                modeDropDown.SetValueWithoutNotify(index);
            }
        }
        private void PopulateResolutionDropDown()
        {
            List<Resolution> resolutions = Screen.resolutions.ToList();

            List<string> resolutionsStrings = new List<string>();
            string currentRes = Screen.width + "x" + Screen.height;
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].height + resolutions[i].width <= minimumResArea)
                {
                    continue;
                }
                string resString = resolutions[i].width + "x" + resolutions[i].height;

                if (resolutionsStrings.Contains(resString))
                {
                    continue;
                }
                resolutionsStrings.Add(resString);
            }
            resolutionsStrings.Reverse();
            resDropDown.ClearOptions();

            resDropDown.AddOptions(resolutionsStrings);
            resDropDown.RefreshShownValue();
            resDropDown.SetValueWithoutNotify(resolutionsStrings.IndexOf(currentRes));
        }

        public void OnResolutionOptionSelected(int value)
        {
            TMP_Dropdown.OptionData option = resDropDown.options[value];
            string optionText = option.text;
            int index = optionText.IndexOf('x');
            int width = Convert.ToInt16(optionText.Substring(0, index));
            int height = Convert.ToInt16(optionText.Substring(index + 1, optionText.Length - index - 1));

            Screen.SetResolution(width, height, Screen.fullScreenMode);
        }
        public void OnModeOptionSelected(int value)
        {
            switch (value)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                default:
                    break;
            }
        }
    }
}