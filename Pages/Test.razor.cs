using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Pages
{
    public partial class Test
    {
        private bool showTickets = false;
        private bool isPlayersSelected = true;
        private bool isWinnersSelected = false;
        private PlayerTickets PlayerTickets;
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
    }
}
