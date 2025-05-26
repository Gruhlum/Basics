using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics.UI;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class TableText
    {
        public List<MultiText> multiTexts = new List<MultiText>();

        public TableText(List<MultiText> multiTexts)
        {
            this.multiTexts = multiTexts;
        }
        public TableText(params MultiText[] multiTexts) : this(multiTexts.ToList())
        {
        }
    }
}