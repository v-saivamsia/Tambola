using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Pages
{
    public partial class AddPlayer
    {
        [Parameter]
        public bool showTickets { get; set; } = false;
        private string showPanel = "";
        private async Task closeTicketsPanel()
        {
            showTickets = false;
            await showTicketsPanel.InvokeAsync(!showTickets);
            Console.WriteLine("close button clicked");
            //StateHasChanged();
        }
        [Parameter]
        public EventCallback<bool> showTicketsPanel { get; set; } 
        protected override async Task OnParametersSetAsync()
        {
            showPanel = showTickets ? "showPanel" : "";
            await base.OnParametersSetAsync();
        }
    }
}
