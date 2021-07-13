using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tambola.Models
{
    public class Ticket
    {
        public int?[,] TicketArray { get; }
        public int[] TicketSet { get; }

        public Ticket()
        {
            TicketArray = new int?[3, 9];
            int twopownine = 1 << 9;
            twopownine--;
            TicketSet = new int[3] {twopownine,twopownine,twopownine };
        }
        public Ticket(string str)
        {
            List<List<int?>> list = JsonSerializer.Deserialize<List<List<int?>>>(str);
            TicketArray = new int?[3, 9];
            int i = 0 , j = 0;
            foreach (List<int?> listRows in list)
            {
                foreach (int? num in listRows)
                {
                    TicketArray[i,j++] = num;
                }
                i++;
                j = 0;
            }
        }
        public static implicit operator string(Ticket ticket)
        {
            List<List<int?>> list = new List<List<int?>>();
            for(int i = 0; i < 3; i++)
            {
                list.Add(new List<int?>());
                for(int j = 0; j < 9; j++)
                    list[i].Add(ticket.TicketArray[i, j]);
            }
            string res = JsonSerializer.Serialize<List<List<int?>>>(list);
            return res;
        }
    }
}
