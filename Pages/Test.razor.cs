using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Pages
{
    public partial class Test
    {
        private bool showTickets = false;
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
    }
}
