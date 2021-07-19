using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Tambola.Services;
using Tambola.Models;
using Tambola.Constants;
using Microsoft.JSInterop;

namespace Tambola.Pages
{
    public partial class DisplayTickets
    {
        [Parameter]
        public int NumberOfTickets { get; set; } = 1;
        [Parameter]
        public bool isFullHeight { get; set; } = false;
        [Parameter]
        public bool isRealPlayer { get; set; } = false;
        [Inject]
        private IJSRuntime jSRuntime { get; set; }
        [Inject]
        private ILocalStorageService localStorage { get; set; }
        [Inject]
        private AvailableWinningWays availableWinningWays { get; set; }

        [Inject]
        private TicketFactory ticketFactory { get; set; }
        public PlayerTicket playerTicket { get; set; } 
        private TicketManager ticketManager;
        private bool showWinButtons = false;
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
        private void deletePlayer()
        {
            removePlayer.InvokeAsync();
        }
        [Parameter]
        public EventCallback removePlayer { get; set; }
        private void markWinner()
        {
            showWinButtons = !showWinButtons;
        }
        private void winButtonPressed(string s)
        {
            markPlayerAsWinner.InvokeAsync(s);
            showWinButtons = false;
        }
        [Parameter]
        public EventCallback<string> markPlayerAsWinner { get; set; }
        private void AddTicket()
        {
            NumberOfTickets++;
        }
        private void DeleteTicket()
        {
            NumberOfTickets--;
        }
        private async Task copyTickets(string id)
        {
            await jSRuntime.InvokeVoidAsync("copyTickets", id);
        }
    }
}
