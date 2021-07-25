using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Services;

namespace Tambola.Pages
{
    public partial class Winners : IDisposable
    {
        [Inject]
        public IJSRuntime jsRuntime { get; set; }
        [Inject]
        public ILocalStorageService localStorage { get; set; }
        [Inject]
        public MarkedWinners markedWinners { get; set; }
        [Inject]
        public ComponentService ComponentService { get; set; }
        [Inject]
        public NotificationService notificationService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            ComponentService.winners = this;
            await base.OnInitializedAsync();
            notificationService.Notify += clear;
        }

        private async Task clear()
        {
            await InvokeAsync(async () =>
            {
                await markedWinners.GetInitialWinnersHelper();
                StateHasChanged();
            });
        }
        private string getWinner(string s, int index)
        {
            List<string> winners = markedWinners.winners[markedWinners.availableWinningWays.dictionary[s]];
            int count = winners.Count;
            if (count > index) return winners[index];
            return "No winner yet";
        }
        public void statehaschanged() { StateHasChanged(); }
        private async Task clearWinner(string s)
        {
            bool isCleared = await jsRuntime.InvokeAsync<bool>("confirmfunction", $"Are you sure you want to clear all the winners in {s}");
            if (isCleared)
            {
                markedWinners.winners[markedWinners.availableWinningWays.dictionary[s]].Clear();
                await localStorage.SetItemAsync<List<List<string>>>("Winners", markedWinners.winners);
            }
        }

        public void Dispose()
        {
            notificationService.Notify -= clear;
        }
    }
}
