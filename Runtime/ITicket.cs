using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics
{
    public interface ITicket<T> where T : ITicket<T>
    {
        public int Tickets
        {
            get;
        }
        public static T Roll(List<T> ticketMasters)
        {
            if (ticketMasters == null || ticketMasters.Count == 0)
            {
                return default;
            }

            int totalTickets = ticketMasters.Sum(x => x.Tickets);

            if (totalTickets <= 0)
            {
                Debug.Log("No Tickets!");
                return default;
            }

            int rng = Random.Range(0, totalTickets);

            foreach (var ticketMaster in ticketMasters)
            {
                if (ticketMaster.Tickets > rng)
                {
                    return ticketMaster;
                }
                else rng -= ticketMaster.Tickets;
            }

            Debug.Log("Could not determine result!");
            return ticketMasters[0];
        }
    }
    public static class ITicketExtensions
    {
        public static T Roll<T>(this List<T> ticketMasters) where T : ITicket<T>
        {
            return ITicket<T>.Roll(ticketMasters);
        }
    }
}