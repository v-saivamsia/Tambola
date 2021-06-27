using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Models
{
    public class Ticket
    {
        public int[,] TicketArray { get; }
        public List<List<int>> AvailableIndices { get; }
        public int[] TicketSet { get; }

        public Ticket()
        {
            TicketArray = new int[3, 9];
            int twopownine = 1 << 9;
            twopownine--;
            TicketSet = new int[3] {twopownine,twopownine,twopownine };
            AvailableIndices = new List<List<int>>();
            for(int i = 0;i < 3; i++)
            {
                AvailableIndices.Add(new List<int>());
                for(int j = 0;j< 9; j++)
                    AvailableIndices[i].Add(j);
            }
        }
    }
}
