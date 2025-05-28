using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames
{
    public class TableDisplay : GroupTextDisplay<MultiTextDisplay, MultiText, TableDisplay, TableText>
    {
        [SerializeField] private TableController tableController = default;

        protected override void DrawItem(TableText items)
        {
            CreateSubDisplays(Item.multiTexts);
            SetControllerItems();
        }

        private void SetControllerItems()
        {
            if (tableController == null)
            {
                return;
            }

            List<HorizontalOrVerticalLayoutGroup> layoutGroups = new List<HorizontalOrVerticalLayoutGroup>();
            foreach (var display in spawner)
            {
                layoutGroups.Add(display.GetComponent<HorizontalOrVerticalLayoutGroup>());
            }
            tableController.SetContentItems(layoutGroups);
        }
    }
}