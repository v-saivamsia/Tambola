using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Constants;
using Tambola.Models;
using Tambola.Services;

namespace Tambola.Pages
{
    public partial class PlayerTickets : IDisposable
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
        [Inject]
        public NotificationService notificationService { get; set; }
        [Inject]
        public IJSRuntime jSRuntime { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await getPlayerNames();
            await base.OnInitializedAsync();
            componentService.playerTickets = this;
            notificationService.Notify += clear;
        }
        public void Dispose()
        {
            notificationService.Notify -= clear;
        }
        public void playerSelected(string name)
        {
            selectedPlayer = name;
            displayTickets.setTicketManagerExternal(playerTickets[name]);
            StateHasChanged();
        }
        public void statechanged(Tuple<string, PlayerTicket> tuple)
        {
            AddPlayer(tuple.Item1, tuple.Item2);
            _bodyTemplate.statechanged();
        }
        private async Task getPlayerNames()
        {
            _players = new List<string>();
            int count = await localStorage.LengthAsync();
            for (int i = 0; i < count; i++)
            {
                _players.Add(await localStorage.KeyAsync(i));
                if (_players[i].Equals("Winners")||_players[i].Equals("PickedNumbers"))
                {
                    continue;
                }
                playerTickets.Add(_players[i], new PlayerTicket(await localStorage.GetItemAsync<PlayerTicket>(_players[i])));
            }
            _players.Remove("Winners");
            _players.Remove("PickedNumbers");
            _players.Sort();
        }
        public void AddPlayer(string name, PlayerTicket playerTicket)
        {
            try
            {
                playerTickets.Add(name, new PlayerTicket(playerTicket));
                _players.Add(name);
                _players.Sort();
            }
            catch (Exception)
            {
                Task.Run(async () => await jSRuntime.InvokeVoidAsync("alertfunction", "Player already exists!"));
            }
        }
        public async Task clear()
        {
            await InvokeAsync(() =>
            {
                selectedPlayer = "";
                _players.Clear();
                playerTickets.Clear();
                playerTickets.Add("", new PlayerTicket());
                _bodyTemplate.statechanged();
            });
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
            if (markedWinners.winners[index].Count < 2)
            {
                markedWinners.winners[index].Add(selectedPlayer);
                localStorage.SetItemAsync<List<List<string>>>("Winners", markedWinners.winners);
            }

        }
    }
}
