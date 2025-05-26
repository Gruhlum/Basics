using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class SingleText
    {
        public string text;
        public List<string> linkTexts;


        public SingleText(string text)
        {
            this.text = text;
        }
        public SingleText(string text, List<string> linkTexts) : this(text)
        {
            this.linkTexts = linkTexts;
        }
        public SingleText(string text, params string[] linkTexts) : this(text, linkTexts.ToList())
        {
        }
    }
}