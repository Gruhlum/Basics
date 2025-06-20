using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class MultiTextDisplay : GroupTextDisplay<SingleTextDisplay, SingleText, MultiTextDisplay, MultiText>
    {
        [SerializeField] private TextAlignment leftColumn = TextAlignment.Left;
        [SerializeField] private TextAlignment centerColumn = TextAlignment.Center;
        [SerializeField] private TextAlignment rightColumn = TextAlignment.Right;

        protected override void DrawItem(MultiText multiText)
        {
            CreateSubDisplays(multiText.texts);
        }

        protected override SingleTextDisplay SpawnLowerDisplay(int index, int total)
        {
            SingleTextDisplay display = base.SpawnLowerDisplay(index, total);

            if (index == 0)
            {
                display.TextGUI.alignment = ConvertToAlignmentOptions(leftColumn);
            }
            else if (index + 1 == total)
            {
                display.TextGUI.alignment = ConvertToAlignmentOptions(rightColumn);
            }
            else display.TextGUI.alignment = ConvertToAlignmentOptions(centerColumn);

            return display;
        }

        private TextAlignmentOptions ConvertToAlignmentOptions(TextAlignment alignment)
        {
            if (alignment == TextAlignment.Left)
            {
                return TextAlignmentOptions.Left;
            }
            else if (alignment == TextAlignment.Right)
            {
                return TextAlignmentOptions.Right;
            }
            else if (alignment == TextAlignment.Center)
            {
                return TextAlignmentOptions.Center;
            }
            else return TextAlignmentOptions.Left;
        }
    }
}