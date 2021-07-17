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
        private PlayerTickets PlayerTickets;
        private Winners winners;
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
                StateHasChanged();
            }
        }
        private void playersSelected()
        {
            isPlayersSelected = true;
            isWinnersSelected = false;
        }
        private void winnersSelected()
        {
            isWinnersSelected = true;
            isPlayersSelected = false;
        }
        private void clear()
        {
            try
            {
                bool temp1 = isPlayersSelected, temp2 = isWinnersSelected;
                playersSelected();
                PlayerTickets.clear();
                winnersSelected();
                winners.markedWinners.GetInitialWinnersHelper();
                winners.statehaschanged();
                isModalVisible = false;
                isPlayersSelected = temp1;
                isWinnersSelected = temp2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void showModal() { isModalVisible = true; }
        private void closeModal() { isModalVisible = false; }
        public void publicshowModal() { isModalVisible = true; }
    }
}
