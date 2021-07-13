using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Models
{
    public class PlayerTicket
    {
        public List<string> tickets { get; set; }
        public int numberOftickets { get; set; } = 0;
        public PlayerTicket()
        {

        }
        public PlayerTicket(PlayerTicket playerTicket)
        {
            numberOftickets = playerTicket.numberOftickets;
            tickets = new List<string>(playerTicket.tickets);
        }
        public PlayerTicket(int num,List<string> list)
        {
            numberOftickets = num;
            tickets = new List<string>(list);
        }
    }
}
