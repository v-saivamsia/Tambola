using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Constants;
using Tambola.Models;
using Tambola.Services;

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
        [Inject]
        public ComponentService componentService { get; set; }
        [Inject]
        public AvailableWinningWays availableWinningWays { get; set; }
        [Inject]
        public MarkedWinners markedWinners { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await getPlayerNames();
            await base.OnInitializedAsync();
            componentService.playerTickets = this;
        }
        public void playerSelected(string name)
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
                if (_players[i].Equals("Winners"))
                {
                    continue;
                }
                playerTickets.Add(_players[i], new PlayerTicket(await localStorage.GetItemAsync<PlayerTicket>(_players[i])));
            }
            _players.Remove("Winners");
            _players.Sort();
        }
        public void AddPlayer(string name, PlayerTicket playerTicket)
        {
            _players.Add(name); 
            playerTickets.Add(name, new PlayerTicket(playerTicket));
            _players.Sort();
        }
        public void clear()
        {
            selectedPlayer = "";
            _players.Clear();
            playerTickets.Clear();
            playerTickets.Add("", new PlayerTicket());
            localStorage.ClearAsync();
            componentService.winners.markedWinners.GetInitialWinnersHelper();
            componentService.winners.statehaschanged();
            _bodyTemplate.statechanged();
        }
        private void deletePlayer()
        {
            if (selectedPlayer.Equals("")) return;
            _players.Remove(selectedPlayer);
            localStorage.RemoveItemAsync(selectedPlayer);
            playerTickets.Remove(selectedPlayer);
            selectedPlayer = "";
        }
        private void markPlayerWinner(string s)
        {
            int index = availableWinningWays.dictionary[s];
            if (markedWinners.winners[index].Count >= 2)
            {
                return;
            }
            componentService.winners.markedWinners.winners[index].Add(selectedPlayer);
            localStorage.SetItemAsync<List<List<string>>>("Winners",componentService.winners.markedWinners.winners);
            componentService.winners.statehaschanged();
        }
    }
}
