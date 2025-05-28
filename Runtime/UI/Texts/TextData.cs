using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class TextData
    {
        public List<object> datas = new List<object>();


        public TextData()
        {
        }

        public TextData(params string[] texts)
        {
            foreach (var text in texts)
            {
                datas.Add(new SingleText(text));
            }
        }
        public TextData(params TableText[] tables)
        {
            datas.AddRange(tables);
        }
    }
}