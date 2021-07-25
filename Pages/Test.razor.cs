using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Services;

namespace Tambola.Pages
{
    public partial class Test
    {
        private bool isModalVisible = false;
        private bool showTickets = false;
        private bool isPlayersSelected = true;
        private bool isWinnersSelected = false;
        private bool isNumbersSelected = false;
        private PlayerTickets PlayerTickets;
        private Winners winners;
        [Inject]
        public NotificationService notificationService { get; set; }
        [Inject]
        public ILocalStorageService localStorage { get; set; }
        [Inject]
        public MarkedWinners markedWinners { get; set; }
        [Inject]private NumberPickerManager numberPickerManager { get; set; }
        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        public void statehaschanged() { StateHasChanged(); }
        private void btnClicked()
        {
            showTickets = true;
            Console.WriteLine("btn clicked");
        }
        private void closeButtonInChildClicked(bool isClosed)
        {
            if (isClosed)
            {
                showTickets = false;
            }
        }
        private void playersSelected()
        {
            isPlayersSelected = true;
            isWinnersSelected = false;
            isNumbersSelected = false;
        }
        private void winnersSelected()
        {
            isWinnersSelected = true;
            isPlayersSelected = false;
            isNumbersSelected = false;
        }
        private void numbersSelected()
        {
            isWinnersSelected = false;
            isPlayersSelected = false;
            isNumbersSelected = true;
        }
        private async Task clear()
        {
            await localStorage.ClearAsync();
            await notificationService.Update();
            await markedWinners.GetInitialWinnersHelper();
            closeModal();
        }
        private void showModal() { isModalVisible = true; }
        private void closeModal() { isModalVisible = false; }
        public void publicshowModal() { isModalVisible = true; }
    }
}
