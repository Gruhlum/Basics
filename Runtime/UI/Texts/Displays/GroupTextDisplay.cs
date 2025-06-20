using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class GroupTextDisplay<TLowerDisplay, LowerT, TDisplay, T> :
        BaseTextDisplay<TDisplay, T> where TDisplay : Display<TDisplay, T> where TLowerDisplay : BaseTextDisplay<TLowerDisplay, LowerT>
    {
        [SerializeField] protected Spawner<TLowerDisplay> spawner;

        public override bool HasListener
        {
            get
            {
                return hasListener;
            }
        }
        protected bool hasListener;


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

        protected void CreateSubDisplays(List<LowerT> singleTexts)
        {
            for (int i = 0; i < singleTexts.Count; i++)
            {
                LowerT text = singleTexts[i];
                TLowerDisplay display = SpawnLowerDisplay(i, singleTexts.Count);
                display.SetItem(text);
                if (display.HasListener)
                {
                    AddEvents(display);
                    hasListener = true;
                }
                else hasListener = false;
            }
        }

        protected virtual TLowerDisplay SpawnLowerDisplay(int index, int total)
        {
            return spawner.Spawn();
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