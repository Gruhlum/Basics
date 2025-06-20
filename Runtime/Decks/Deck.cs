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


        public Deck(IList<DeckItem<T>> items)
        {
            ChangeOdds(items);
        }
        public Deck(DeckItem<T> item, params DeckItem<T>[] items)
        {
            ChangeOdds(item, items);
        }

        public void ChangeOdds(DeckItem<T> item, params DeckItem<T>[] items)
        {
            List<DeckItem<T>> results = new List<DeckItem<T>>(items) { item };
            ChangeOdds(results);
        }
        public void ChangeOdds(IList<DeckItem<T>> items)
        {
            totalItems.Clear();

            foreach (var item in items)
            {
                totalItems.Add(item);
            }

            GenerateDeck();
        }

        public bool HasRollsLeft()
        {
            foreach (var item in currentItems)
            {
                if (item.Tickets > 0)
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
            DeckItem<T> deck = ITicket.Roll(currentItems);
            if (deck == null)
            {
                Debug.Log("No Deck available!");
                return default;
            }
            deck.Tickets--;
            if (deck.Tickets <= 0)
            {
                currentItems.Remove(deck);
            }
            return deck.item;
        }


        private string GetItemsToString(List<DeckItem<T>> items)
        {
            List<string> totalItemsStrings = new List<string>();
            foreach (var item in totalItems)
            {
                totalItemsStrings.Add(item.ToString());
            }
            return string.Join(", ", totalItemsStrings);
        }

        public override string ToString()
        {
            return $"Total: {GetItemsToString(totalItems)} || Current: {GetItemsToString(currentItems)}";
        }
    }
}