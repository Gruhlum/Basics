using HexTecGames.Basics;

namespace HexTecGames
{
    [System.Serializable]
    public class DeckItem<T> : ITicket
    {
        public T item;

        public int Tickets
        {
            get
            {
                return this.tickets;
            }
            set
            {
                this.tickets = value;
            }
        }
        private int tickets;

        public DeckItem(T item, int amount)
        {
            this.item = item;
            this.tickets = amount;
        }
        public DeckItem(DeckItem<T> deckItem) : this(deckItem.item, deckItem.Tickets)
        {
        }
        public override string ToString()
        {
            return $"{item}: {Tickets}";
        }
    }
}