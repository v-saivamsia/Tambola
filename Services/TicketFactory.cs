using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Services
{
    public class TicketFactory
    {
        public TicketManager ticketManager {
            get
            {
                return new TicketManager();
            }
        }
    }
}
