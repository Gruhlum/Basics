using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class ValueDescription
    {
        public string description;
        public float value;

        public ValueDescription(string description, float value)
        {
            this.value = value;
            this.description = description;
        }

        public string GetItem()
        {
            return $"{description}: {value}";
        }
    }
}