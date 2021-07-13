using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Tambola.Services;
using Tambola.Models;

namespace Tambola.Pages
{
    public partial class DisplayTickets
    {
        [Parameter]
        public int NumberOfTickets { get; set; } = 1;
        [Parameter]
        public bool isFullHeight { get; set; } = false;

        [Inject]
        private ILocalStorageService localStorage { get; set; }

        [Inject]
        private TicketFactory ticketFactory { get; set; }
        public PlayerTicket playerTicket { get; set; } 
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
        public void saveTickets(string name)
        {
            playerTicket = new PlayerTicket()
            {
                numberOftickets = NumberOfTickets,
                tickets = ticketManager
            };
            localStorage.SetItemAsync<PlayerTicket>(name,playerTicket);
        }
        public void setTicketManagerExternal(PlayerTicket playerTicket)
        {
            ticketManager = new TicketManager(playerTicket);
            StateHasChanged();
        }
    }
}
