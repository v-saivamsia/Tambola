using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.CustomValidation
{
    public class DuplicatePlayerValidator :ComponentBase
    {
        private ValidationMessageStore validationMessageStore;
        [CascadingParameter]
        private EditContext editContext { get; set; }
        [Inject]
        private ILocalStorageService localStorageService { get; set; }
        protected override void OnInitialized()
        {
            validationMessageStore = new(editContext);
            editContext.EnableDataAnnotationsValidation();
        }
    }
}
