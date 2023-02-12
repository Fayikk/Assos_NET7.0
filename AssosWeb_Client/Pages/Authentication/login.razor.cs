using AssosModels;
using AssosWeb_Client.Service.IService;
using Microsoft.AspNetCore.Components;
using System.Web;

namespace AssosWeb_Client.Pages.Authentication
{
    public partial class Login
    {
        private SignInRequestDTO SignInRequest = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowSignInErrors { get; set; }
        public string Errors { get; set; }
        public string ReturnUrl { get; set; }
        [Inject]
        public IAuthenticationService _authenticationService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        private async Task LoginUser()
        {
            ShowSignInErrors = false;
            IsProcessing = true;
            var result = await _authenticationService.Login(SignInRequest);
            if (result.IsAuthSuccessful)
            {
                //regiration is successful
                var absoluteUri = new Uri(_navigationManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];//Login yönlendirmesinde eğer başarılı giriş yapılırsa aynı url'e yönlendirme
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    _navigationManager.NavigateTo("/" + ReturnUrl);
                }
            }
            else
            {
                //failure
                Errors = result.ErrorMessage;
                ShowSignInErrors = true;

            }
            IsProcessing = false;

        }
    }
}

