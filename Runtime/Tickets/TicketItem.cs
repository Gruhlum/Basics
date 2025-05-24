using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class TicketItem<T> : ITicket
    {
        public int Tickets
        {
            get
            {
                return tickets;
            }
            set
            {
                tickets = value;
            }
        }

        public T Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        private int tickets;
        private T item;


        public TicketItem(int tickets, T item)
        {
            this.Tickets = tickets;
            this.Item = item;
        }
    }
}