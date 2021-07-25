using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Models;

namespace Tambola.Services
{
    public class NumberPickerManager
    {
        private readonly ILocalStorageService localStorageService;
        public PickedNumbers pickedNumbers;
        public NumberPickerManager(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
            Task.Run(async () => await setPickedNumbers());
        }
        private async Task setPickedNumbers()
        {
            pickedNumbers = await localStorageService.GetItemAsync<PickedNumbers>("PickedNumbers");
            if(pickedNumbers == null)
            {
                pickedNumbers = new PickedNumbers { PickedNumbersList = new List<int>() { 0,0,0} };
                await localStorageService.SetItemAsync<PickedNumbers>("PickedNumbers", pickedNumbers);
            }
        }
        public async Task setPickedNumber(int number)
        {
            pickedNumbers.PickedNumbersList[number/30]|=1<<number % 30;
            await localStorageService.SetItemAsync<PickedNumbers>("PickedNumbers", pickedNumbers);
        }
        public async Task clearNumbers()
        {
            await localStorageService.RemoveItemAsync("PickedNumbers");
            await setPickedNumbers();
        }
    }
}
