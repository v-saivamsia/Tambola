using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Services;

namespace Tambola.Pages
{
    public partial class NumberPicker : IDisposable
    {
        [Inject]
        public NumberPickerManager numberPickerManager { get; set; }
        private List<int> allNumbers = new List<int>();
        private List<int> availableNumbersToBePicked = new List<int>();
        private bool[] isPickedNumber = new bool[90];
        private int? currentPickedNumber = null;
        [Inject] private IJSRuntime jSRuntime { get; set; }
        [Inject]
        private NotificationService notificationService { get; set; }
        protected override void OnInitialized()
        {
            InitializeNumbers();
            notificationService.Notify += OnNotify;
        }
        private async Task OnNotify()
        {
            await InvokeAsync(async () =>
            {
                await clear();
                StateHasChanged();
            });
        }
        private void InitializeNumbers()
        {
            for (int i = 0; i < 90; i++)
            {
                allNumbers.Add(i);
                if ((numberPickerManager.pickedNumbers.PickedNumbersList[i / 30] & (1 << i % 30)) == 0)
                {
                    availableNumbersToBePicked.Add(i);
                }
                else isPickedNumber[i] = true;
            }
            shuffleList(availableNumbersToBePicked);
            shuffleList(availableNumbersToBePicked);
            shuffleList(availableNumbersToBePicked);
        }
        private async Task pickNumber()
        {
            if (availableNumbersToBePicked.Count == 0)
            {
                await jSRuntime.InvokeVoidAsync("alertfunction", "All numbers have been picked!");
                return;
            }
            int last = availableNumbersToBePicked[availableNumbersToBePicked.Count - 1];
            await numberPickerManager.setPickedNumber(last);
            availableNumbersToBePicked.RemoveAt(availableNumbersToBePicked.Count - 1);
            isPickedNumber[last] = true;
            currentPickedNumber = last;
        }
        private void shuffleList(List<int> list)
        {
            Random random = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int index = random.Next(0, i);
                int temp = list[index];
                list[index] = list[i];
                list[i] = temp;
            }
        }
        private async Task shouldClear()
        {
            bool shouldClear = await jSRuntime.InvokeAsync<bool>("confirmfunction", "Are you sure you want to clear all the numbers");

            if (shouldClear)
            {
                await clear();
            }
        }
        private async Task clear()
        {
            isPickedNumber = new bool[90];
            await numberPickerManager.clearNumbers();
            allNumbers.Clear();
            InitializeNumbers();
            currentPickedNumber = null;
        }

        public void Dispose()
        {
            notificationService.Notify -= OnNotify;
        }
    }
}
