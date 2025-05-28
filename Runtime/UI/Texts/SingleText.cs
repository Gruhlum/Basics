using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class SingleText
    {
        [TextArea] public string text;
        public List<TextData> linkTexts = new List<TextData>();

        public SingleText(string text)
        {
            this.text = text;
        }
        public SingleText(string text, params TextData[] textDatas) : this(text)
        {
            linkTexts = textDatas.ToList();
        }
        public SingleText(string text, List<string> linkTexts) : this(text)
        {
            foreach (var linkText in linkTexts)
            {
                this.linkTexts.Add(new TextData(linkText));
            }
        }
        public SingleText(string text, params string[] linkTexts) : this(text, linkTexts.ToList())
        {
        }
    }
}