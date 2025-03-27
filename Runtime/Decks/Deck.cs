using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{


    [System.Serializable]
    public class Deck<T>
    {
        protected List<DeckItem<T>> totalItems = new List<DeckItem<T>>();
        protected List<DeckItem<T>> currentItems = new List<DeckItem<T>>();


        public Deck(List<DeckItem<T>> items)
        {
            foreach (var item in items)
            {
                totalItems.Add(item);
            }

            GenerateDeck();
        }
        public Deck(params DeckItem<T>[] items) : this(items.ToList())
        {
        }

        private DeckItem<T> RollDeckItem()
        {
            int totalTickets = currentItems.Max(x => x.amount);
            int rng = Random.Range(0, totalTickets);
            foreach (var item in currentItems)
            {
                if (rng < item.amount)
                {
                    return item;
                }
                rng -= item.amount;
            }
            Debug.Log("Error: Could not determine a valid deck!");
            return currentItems[0];
        }

        public bool HasRollsLeft()
        {
            foreach (var item in currentItems)
            {
                if (item.amount > 0)
                {
                    return true;
                }
            }
            return false;
        }
        protected void GenerateDeck()
        {
            currentItems = new List<DeckItem<T>>(totalItems);
        }

        public T GetNext()
        {
            if (currentItems.Count == 0)
            {
                GenerateDeck();
            }
            DeckItem<T> deck = RollDeckItem();
            deck.amount--;
            if (deck.amount <= 0)
            {
                currentItems.Remove(deck);
            }
            return deck.item;
        }       
    }
}