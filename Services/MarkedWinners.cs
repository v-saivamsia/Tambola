using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Constants;

namespace Tambola.Services
{
    public class MarkedWinners
    {
        private readonly ILocalStorageService localStorageService;
        private readonly AvailableWinningWays availableWinningWays;
        public List<List<string>> winners;
        public MarkedWinners(ILocalStorageService localStorageService,AvailableWinningWays availableWinningWays)
        {
            this.localStorageService = localStorageService;
            this.availableWinningWays = availableWinningWays;
            Task.Run(async ()=> await GetInitialWinners());
        }
        private async Task GetInitialWinners()
        {
            winners = await localStorageService.GetItemAsync<List<List<string>>>("Winners");
            if (winners == null)
            {
                winners = new List<List<string>>();
                foreach (string s in availableWinningWays.list)
                {
                    winners.Add(new List<string>());
                }
            }
        }
    }
}
