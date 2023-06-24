using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessenger.Domain.AppLogic.Extensions;
using SharpMessenger.Presentation.Shared;
using System.Security.Claims;

namespace SharpMessenger.Domain.AppLogic.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService SessionStorage;
        private ClaimsPrincipal Anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage)
        {
            SessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                UserSession userSession = await SessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");

                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(Anonymous));
                }

                ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(Anonymous));
            }
        }

        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role),
                    new Claim(ClaimTypes.NameIdentifier, userSession.UserName)
                }));

                userSession.ExpiryTimeStamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
                await SessionStorage.SaveItemEncryptedAsync("UserSession", userSession);
            }
            else
            {
                claimsPrincipal = Anonymous;
                await SessionStorage.RemoveItemAsync("UserSession");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<string> GetToken()
        {
            string result = string.Empty;

            try
            {
                UserSession userSession = await SessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now < userSession.ExpiryTimeStamp)
                    result = userSession.Token;
            }
            catch { }

            return result;
        }
    }
}
