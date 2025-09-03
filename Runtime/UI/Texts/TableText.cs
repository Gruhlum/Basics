using System;
using System.Collections.Generic;
using System.Linq;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class TableText
    {
        public List<MultiText> multiTexts = new List<MultiText>();


        public TableText() { }
        public TableText(params string[] texts) : this(new MultiText(texts))
        {
        }
        public TableText(List<MultiText> multiTexts)
        {
            this.multiTexts = multiTexts;
        }
        public TableText(params MultiText[] multiTexts) : this(multiTexts.ToList())
        {
        }

        public override string ToString()
        {
            if (multiTexts == null || multiTexts.Count < 0)
            {
                return "[Empty TableText]";
            }

            List<string> textDescriptions = new List<string>();
            foreach (MultiText text in multiTexts)
            {
                textDescriptions.Add(text.ToString());
            }

            return string.Join(Environment.NewLine, textDescriptions);
        }
    }
}