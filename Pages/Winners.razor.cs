using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Services;

namespace Tambola.Pages
{
    public partial class Winners
    {
        [Inject]
        public MarkedWinners markedWinners { get; set; }
        [Inject]
        public ComponentService ComponentService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            ComponentService.winners = this;
            await base.OnInitializedAsync();
        }
        private string getWinner(string s,int index)
        {
            List<string> winners = markedWinners.winners[markedWinners.availableWinningWays.dictionary[s]];
            int count = winners.Count;
            if(count>index) return winners[index];
            return "No winner yet";
        }
        public void statehaschanged() { StateHasChanged(); }
    }
}
