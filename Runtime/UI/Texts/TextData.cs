using System;
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

        public void Add(object obj)
        {
            datas.Add(obj);
        }


        public override string ToString()
        {
            List<string> allTexts = new List<string>();
            foreach (var item in datas)
            {
                allTexts.Add(item.ToString());
            }

            return $"Total Items: {datas.Count}{Environment.NewLine}{string.Join(Environment.NewLine, allTexts)}";
        }
    }
}