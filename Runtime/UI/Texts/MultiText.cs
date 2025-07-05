using System.Collections.Generic;
using System.Linq;

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
            foreach (string text in texts)
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

        public override string ToString()
        {
            if (texts == null || texts.Count < 0)
            {
                return "[Empty MultiText]";
            }

            List<string> textDescriptions = new List<string>();

            foreach (SingleText text in texts)
            {
                textDescriptions.Add(text.ToString());
            }

            return $"{string.Join(" | ", textDescriptions)}";
        }
    }
}