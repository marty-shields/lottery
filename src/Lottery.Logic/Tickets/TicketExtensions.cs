using Lottery.Core.Entities;

namespace Lottery.Logic.Tickets
{
    public static class TicketExtensions
    {
        public static void RemoveTicketsFromTicketPool(this List<Ticket> ticketPool, IEnumerable<Ticket> ticketsToRemove)
        {
            foreach (var ticketToRemove in ticketsToRemove)
            {
                ticketPool.RemoveTicketFromTicketPool(ticketToRemove);
            }
        }

        public static void RemoveTicketFromTicketPool(this List<Ticket> ticketPool, Ticket ticketToRemove)
        {
            ticketPool.Remove(ticketToRemove);
        }
    }
}