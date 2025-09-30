using System;
using System.Collections.Generic;
using System.Text;

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
            foreach (string text in texts)
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
            if (obj is TextData textData)
            {
                datas.AddRange(textData.datas);
            }
            else if (obj is string text)
            {
                datas.Add(new SingleText(text));
            }
            else datas.Add(obj);
        }
        public string ToSimpleString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var data in datas)
            {
                stringBuilder.Append(data.ToString());
            }
            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            List<string> allTexts = new List<string>();
            foreach (object item in datas)
            {
                allTexts.Add(item.ToString());
            }

            return $"Total Items: {datas.Count}{Environment.NewLine}{string.Join(Environment.NewLine, allTexts)}";
        }
    }
}