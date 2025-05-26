using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    public interface ITicket
    {
        public int Tickets
        {
            get;
        }
        public static T Roll<T>(List<T> items) where T : ITicket
        {
            if (items == null || items.Count == 0)
            {
                Debug.Log("No items to roll!");
                return default;
            }

            int totalTickets = items.Sum(x => x.Tickets);

            if (totalTickets <= 0)
            {
                Debug.Log("No Tickets!");
                return default;
            }

            int rng = Random.Range(0, totalTickets);

            foreach (var item in items)
            {
                if (item.Tickets > rng)
                {
                    return item;
                }
                else rng -= item.Tickets;
            }

            Debug.Log("Could not determine result!");
            return items[0];
        }
    }
}