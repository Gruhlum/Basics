using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class TextDataDisplay : Display<TextDataDisplay, TextData>
    {
        [SerializeField] private Spawner<SingleTextDisplay> textSpawner = default;
        [SerializeField] private Spawner<MultiTextDisplay> multiTextSpawner = default;
        [SerializeField] private Spawner<TableDisplay> tableSpawner = default;
        [SerializeField] private TextDataDisplay tooltipDisplay = default;
        

        public override void SetItem(TextData item, bool activate = true)
        {
            if (this.Item != null)
            {
                DeactivateAll();

                foreach (var display in textSpawner)
                {
                    RemoveEvents(display);
                }
                foreach (var display in multiTextSpawner)
                {
                    RemoveEvents(display);
                }
                foreach (var table in tableSpawner)
                {
                    RemoveEvents(table);
                }
            }

            base.SetItem(item, activate);
        }

        protected override void DrawItem(TextData fullText)
        {
            foreach (var data in fullText.datas)
            {
                if (data is SingleText singleText)
                {
                    SingleTextDisplay display = textSpawner.Spawn();
                    display.SetItem(singleText);
                    AddEvents(display);
                }
                else if (data is MultiText multiText)
                {
                    MultiTextDisplay display = multiTextSpawner.Spawn();
                    display.SetItem(multiText);
                    AddEvents(display);
                }
                else if (data is TableText tableText)
                {
                    TableDisplay table = tableSpawner.Spawn();
                    table.SetItem(tableText);
                    AddEvents(table);
                }
            }
        }

        private void AddEvents(ILinkListener display)
        {
            if (tooltipDisplay == null)
            {
                return;
            }
            if (!display.HasListener)
            {
                return;
            }
            display.OnLinkHover += LinkListener_OnLinkHover;
            display.OnHoverStopped += LinkListener_OnHoverStopped;
        }
        private void RemoveEvents(ILinkListener linkListener)
        {
            linkListener.OnLinkHover -= LinkListener_OnLinkHover;
            linkListener.OnHoverStopped -= LinkListener_OnHoverStopped;
        }
        private void LinkListener_OnLinkHover(LinkListener linkListener, TextData linkText)
        {
            tooltipDisplay.SetItem(linkText);
        }
        private void LinkListener_OnHoverStopped(LinkListener linkListener)
        {
            tooltipDisplay.Deactivate();
        }
        private void DeactivateAll()
        {
            textSpawner.DeactivateAll();
            multiTextSpawner.DeactivateAll();
            tableSpawner.DeactivateAll();
        }
    }
}