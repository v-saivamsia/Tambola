using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Models;

namespace Tambola.Pages
{
    public partial class PlayerTickets
    {
        private BodyTemplate _bodyTemplate;
        private string selectedPlayer = "";
        private DisplayTickets displayTickets;
        private List<string> _players;
        private Dictionary<string, PlayerTicket> playerTickets = new Dictionary<string, PlayerTicket>() { { "", new PlayerTicket() } };
        [Inject]
        public ILocalStorageService localStorage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await getPlayerNames();
            await base.OnInitializedAsync();
        }
        private void playerSelected(string name)
        {
            selectedPlayer = name;
            displayTickets.setTicketManagerExternal(playerTickets[name]);
            StateHasChanged();
        }
        public void statechanged(Tuple<string,PlayerTicket> tuple)
        {
            AddPlayer(tuple.Item1,tuple.Item2);
            _bodyTemplate.statechanged();
        }
        private async Task getPlayerNames()
        {
            _players = new List<string>();
            int count = await localStorage.LengthAsync();
            for (int i = 0; i < count; i++)
            {
                _players.Add(await localStorage.KeyAsync(i));
                playerTickets.Add(_players[i], new PlayerTicket(await localStorage.GetItemAsync<PlayerTicket>(_players[i])));
            }
            _players.Sort();

        }
        public void AddPlayer(string name, PlayerTicket playerTicket)
        {
            _players.Add(name); 
            playerTickets.Add(name, new PlayerTicket(playerTicket));
            _players.Sort();
        }
    }
}
