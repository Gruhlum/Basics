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
        protected override void DrawItem(MultiText multiText)
        {
            foreach (var text in multiText.texts)
            {
                SingleTextDisplay display = spawner.Spawn();
                display.SetItem(text);
                if (text.linkTexts != null && display.LinkListener != null)
                {
                    AddEvents(display);
                }
            }
        }
    }
}