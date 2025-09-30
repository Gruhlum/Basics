using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class TextDataDisplay : Display<TextDataDisplay, TextData>
    {
        [SerializeField] private TextDataDisplay tooltipDisplay = default;
        [Space]
        [SerializeField] private Spawner<SingleTextDisplay> textSpawner = default;
        [SerializeField] private Spawner<MultiTextDisplay> multiTextSpawner = default;
        [SerializeField] private Spawner<TableDisplay> tableSpawner = default;
        [SerializeField] private Spawner<IconDisplay> iconSpawner = default;

        public override void SetItem(TextData item, bool activate = true)
        {
            if (this.Item != null)
            {
                DeactivateAll();

                foreach (SingleTextDisplay display in textSpawner)
                {
                    RemoveEvents(display);
                }
                foreach (MultiTextDisplay display in multiTextSpawner)
                {
                    RemoveEvents(display);
                }
                foreach (TableDisplay table in tableSpawner)
                {
                    RemoveEvents(table);
                }
            }

            base.SetItem(item, activate);
        }

        protected override void DrawItem(TextData fullText)
        {
            foreach (object data in fullText.datas)
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
                else if (data is IconData iconData)
                {
                    IconDisplay display = iconSpawner.Spawn();
                    display.SetItem(iconData);
                }
                else Debug.Log("Invalid type: " + data.GetType());
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