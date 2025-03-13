using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    [System.Serializable]
    public struct DeckItem<T>
    {
        public int amount;
        public T item;

        public DeckItem(T item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}