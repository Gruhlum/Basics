using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class GroupTextDisplay<TLowerDisplay, LowerT, TDisplay, T> : 
        BaseTextDisplay<TDisplay, T> where TDisplay : Display<TDisplay, T> where TLowerDisplay : BaseTextDisplay<TLowerDisplay, LowerT>
    {
        [SerializeField] protected Spawner<TLowerDisplay> spawner = default;


        protected override void OnDestroy()
        {
            RemoveAllEvents();
            base.OnDestroy();
        }

        private void RemoveAllEvents()
        {
            foreach (var text in spawner)
            {
                RemoveEvents(text);
            }
        }

        public override void SetItem(T item, bool activate = true)
        {
            if (this.Item != null)
            {
                RemoveAllEvents();
                spawner.DeactivateAll();
            }

            base.SetItem(item, activate);
        }

        protected void AddEvents(TLowerDisplay display)
        {
            display.OnLinkHover += LinkHover;
            display.OnHoverStopped += HoverStopped;
        }
        private void RemoveEvents(TLowerDisplay display)
        {
            display.OnLinkHover -= LinkHover;
            display.OnHoverStopped -= HoverStopped;
        }
    }
}