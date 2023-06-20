using Microsoft.AspNetCore.Components.Authorization;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic.MainWindowComponents
{
    public interface IGetAuthState
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
    }
}
