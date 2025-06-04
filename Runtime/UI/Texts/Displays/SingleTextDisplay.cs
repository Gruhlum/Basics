using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class SingleTextDisplay : BaseTextDisplay<SingleTextDisplay, SingleText>
    {
        [SerializeField] private TMP_Text textGUI = default;
        [SerializeField] private LinkListener linkListener = default;
        [SerializeField] private LinkHighlighter linkHighlighter = default;

        public LinkListener LinkListener
        {
            get
            {
                return this.linkListener;
            }
            private set
            {
                this.linkListener = value;
            }
        }

        public override bool HasListener
        {
            get
            {
                return Item != null && Item.linkTexts != null && Item.linkTexts.Count > 0 && LinkListener != null;
            }
        }

        protected void Start()
        {
            if (LinkListener != null)
            {
                LinkListener.OnLinkHover += LinkHover;
                LinkListener.OnHoverStopped += HoverStopped;
            }
        }
        protected override void OnDestroy()
        {
            if (LinkListener != null)
            {
                LinkListener.OnLinkHover -= LinkHover;
                LinkListener.OnHoverStopped -= HoverStopped;
            }
            base.OnDestroy();
        }

        protected override void DrawItem(SingleText item)
        {
            textGUI.SetText(item.text);
            if (LinkListener != null && item.linkTexts != null)
            {
                LinkListener.Setup(item.linkTexts);
            }
            if (false && linkHighlighter != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(Delayed());
            }
        }
        private IEnumerator Delayed()
        {
            yield return null;
            linkHighlighter.Setup(textGUI);
        }
    }
}