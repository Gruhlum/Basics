using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class MultiText
    {
        public List<SingleText> texts = new List<SingleText>();


        public MultiText(params string[] texts) : this(texts.ToList())
        {
        }
        public MultiText(List<string> texts)
        {
            foreach (var text in texts)
            {
                this.texts.Add(new SingleText(text));
            }
        }
        public MultiText(List<SingleText> linkTexts)
        {
            this.texts = linkTexts;
        }
        public MultiText(params SingleText[] linkTexts) : this(linkTexts.ToList())
        {
        }
    }
}