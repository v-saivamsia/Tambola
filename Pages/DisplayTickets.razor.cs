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
        private TicketFactory ticketFactory { get; set; }
        private TicketManager ticketManager;
        public void setTicketManager()
        {
            ticketManager = ticketFactory.ticketManager;
            StateHasChanged();
        }
        protected override void OnInitialized()
        {
            setTicketManager();
            base.OnInitialized();
        }
    }
}
