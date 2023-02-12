using AssosWeb_Client.Service.IService;
using Microsoft.AspNetCore.Components;

namespace AssosWeb_Client.Pages.Authentication
{
    public partial class Logout
    {
        [Inject]
        public IAuthenticationService _authenticationService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await _authenticationService.Logout();
            _navigationManager.NavigateTo("/");
        }


    }
}
