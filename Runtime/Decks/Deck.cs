using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{


    [System.Serializable]
    public class Deck<T>
    {
        private Stack<T> cards = new Stack<T>();

        private List<DeckItem<T>> totalItems = new List<DeckItem<T>>();

        public Deck(params DeckItem<T>[] items)
        {
            foreach (var item in items)
            {
                totalItems.Add(item);
            }

            Shuffle();
        }

        public T RevealNextCard()
        {
            if (cards.Count == 0)
            {
                Shuffle();
            }
            return cards.Pop();
        }

        public void Shuffle()
        {
            List<T> results = new List<T>();
            foreach (var itemList in totalItems)
            {
                for (int i = 0; i < itemList.amount; i++)
                {
                    results.Add(itemList.item);
                }
            }
            results.Shuffle();
            cards = new Stack<T>(results);
        }
    }
}