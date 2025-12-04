using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace HexTecGames.UI
{
    public class ResolutionDropdown : DropdownControl
    {
        [SerializeField] private bool useCustomResolutions = default;
        [DrawIf(nameof(useCustomResolutions), true)]
        [SerializeField] private List<SimpleResolution> customResolutions = new List<SimpleResolution>();

        [SerializeField] private bool allowAnyResolution = true;
        [DrawIf(nameof(allowAnyResolution), false)]
        [SerializeField]
        private List<float> allowedRatios = new List<float> { 16f / 9f, 4f / 3f };

        private List<Resolution> resolutions = new List<Resolution>();


        protected override void Awake()
        {
            base.Awake();

            var currentResolution = Screen.currentResolution;

            var result = resolutions.Find(x => x.height == currentResolution.height && x.width == currentResolution.width);
            int index = resolutions.IndexOf(result);
            if (index < 0)
            {
                Debug.Log("Could not find resolution: " + currentResolution);
                return;
            }
            dropdown.SetValueWithoutNotify(index);
        }

        protected override void PopulateDropdown()
        {
            dropdown.ClearOptions();

            if (useCustomResolutions)
            {
                AddCustomResolutions();
            }
            else
            {
                AddScreenResolutions();
            }
        }

        private void AddCustomResolutions()
        {
            foreach (var customResolution in customResolutions)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(customResolution.ToString()));
                resolutions.Add(customResolution.ToResolution());
            }
        }

        private void AddScreenResolutions()
        {
            resolutions = new List<Resolution>(Screen.resolutions);

            foreach (var resolution in resolutions)
            {
                if (!allowAnyResolution && !IsAllowedRatio(resolution))
                {
                    continue;
                }
                string resolutionString = $"{resolution.width}x{resolution.height}";
                if (dropdown.options.Any(x => x.text == resolutionString))
                {
                    continue;
                }
                dropdown.options.Add(new TMP_Dropdown.OptionData(resolutionString));
            }
        }

        private bool IsAllowedRatio(Resolution resolution)
        {
            foreach (float allowed in allowedRatios)
            {
                if (Mathf.Abs((resolution.width / resolution.height) - allowed) < 0.1f)
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnDropdownChanged(int index)
        {
            var selected = resolutions[index];
            Screen.SetResolution(selected.width, selected.height, Screen.fullScreenMode);
        }
    }
}