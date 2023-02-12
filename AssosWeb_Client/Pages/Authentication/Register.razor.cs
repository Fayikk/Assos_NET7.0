using AssosModels;
using AssosWeb_Client.Service.IService;
using Microsoft.AspNetCore.Components;

namespace AssosWeb_Client.Pages.Authentication
{
    public partial class Register
    {
        private SignUpRequestDTO SignUpRequest = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }
        [Inject]
        public IAuthenticationService _authenticationService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        private async Task RegisterUser()
        {
            ShowRegistrationErrors = false;
            IsProcessing = true;
            var result = await _authenticationService.RegisterUser(SignUpRequest);
            if (result.IsRegisterationSuccessful)
            {
                //registration succesfull    
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                //failure
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
            IsProcessing = false;

        }
    }
}
