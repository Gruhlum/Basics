using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{


    [System.Serializable]
    public class Deck<T>
    {
        protected readonly List<DeckItem<T>> totalItems = new List<DeckItem<T>>();
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

            foreach (DeckItem<T> item in items)
            {
                totalItems.Add(item);
            }

            GenerateDeck();
        }

        public bool HasRollsLeft()
        {
            foreach (DeckItem<T> item in currentItems)
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
            currentItems = new List<DeckItem<T>>(totalItems.Count);
            foreach (var item in totalItems)
            {
                currentItems.Add(new DeckItem<T>(item));
            }
        }

        public T GetNext()
        {
            if (currentItems.Count <= 0)
            {
                GenerateDeck();
            }
            DeckItem<T> deckItem = ITicket.Roll(currentItems);
            if (deckItem == null)
            {
                Debug.Log("No Item available!");
                return default;
            }
            deckItem.Tickets--;
            if (deckItem.Tickets <= 0)
            {
                currentItems.Remove(deckItem);
                //Debug.Log("Removing: " + deckItem.item.ToString());
            }
            return deckItem.item;
        }

        public int GetRemainingTickets(T t)
        {
            var result = currentItems.Find(x => x.item.Equals(t));
            if (result == null)
            {
                return 0;
            }
            return result.Tickets;
        }

        private string GetItemsToString(List<DeckItem<T>> items)
        {
            List<string> totalItemsStrings = new List<string>();
            foreach (DeckItem<T> item in totalItems)
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