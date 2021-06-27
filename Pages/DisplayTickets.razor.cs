using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Services;

namespace Tambola.Pages
{
    public partial class DisplayTickets
    {
        [Parameter]
        public int NumberOfTickets { get; set; } = 1;
        [Inject]
        private TicketManager ticketManager { get; set; }
    }
}
