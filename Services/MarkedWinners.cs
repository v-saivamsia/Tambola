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
        public readonly AvailableWinningWays availableWinningWays = new AvailableWinningWays();
        public List<List<string>> winners;
        public MarkedWinners(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
            Task.Run(async () => await GetInitialWinners());
        }
        private async Task GetInitialWinners()
        {
            winners = await localStorageService.GetItemAsync<List<List<string>>>("Winners");
            if (winners == null)
            {
                await GetInitialWinnersHelper(); 
                await localStorageService.SetItemAsync<List<List<string>>>("Winners", winners);
            }
        }
        public Task GetInitialWinnersHelper()
        {
            winners = new List<List<string>>();
            foreach (string s in availableWinningWays.list)
            {
                winners.Add(new List<string>());
            }

            return Task.CompletedTask;
        }
    }
}
