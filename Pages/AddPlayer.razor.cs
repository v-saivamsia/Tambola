using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Models;

namespace Tambola.Pages
{
    public partial class AddPlayer
    {
        private DisplayTickets displayTickets;
        private Player player = new Player();
        private bool showAllTickets = false;

        [Parameter]
        public bool showTickets { get; set; } = false;
        private string showPanel = "";
        private async Task closeTicketsPanel()
        {
            showTickets = false;
            await showTicketsPanel.InvokeAsync(!showTickets);
            Console.WriteLine("close button clicked");
        }
        [Parameter]
        public EventCallback<bool> showTicketsPanel { get; set; } 
        protected override async Task OnParametersSetAsync()
        {
            showPanel = showTickets ? "showPanel" : "";
            await base.OnParametersSetAsync();
        }

        private void GenerateTickets()
        {
            showAllTickets = true;
        }
        private void reset()
        {
            showAllTickets = false;
            player = new Player();
            if(displayTickets!=null) regenerateTickets();
        }
        private void regenerateTickets()
        {
            displayTickets.setTicketManager();
        }
        private void saveTickets()
        {
            displayTickets.saveTickets(player.Name);
            // display pop up saved tickets of the player
            reset();
        }
    }
}

