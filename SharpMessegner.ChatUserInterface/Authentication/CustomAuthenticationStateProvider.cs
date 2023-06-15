using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessegner.ChatUserInterface.Extensions;
using SharpMessenger.Presentation.Shared;
using System.Security.Claims;

namespace SharpMessegner.ChatUserInterface.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService sessionStorageService = null!;

        private ClaimsPrincipal anonymousUser = new(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ISessionStorageService storageService)
        {
            sessionStorageService = storageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await sessionStorageService.ReadEncryptedItemAsync<UserSession>("UserSession");

                if (userSession == null!)
                {
                    return await Task.FromResult(new AuthenticationState(anonymousUser));
                }

                var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, userSession.UserName),
                    new(ClaimTypes.Role, userSession.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "JwtAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymousUser));
            }
        }

        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal = null!;

            if(userSession != null)
            {
                var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, userSession.UserName),
                    new(ClaimTypes.Role, userSession.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims);
                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                userSession.ExpiryTimeStamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
                await sessionStorageService.SaveItemEncryptedAsync("UserSession", userSession);
            }
            else
            {
                claimsPrincipal = anonymousUser;

                await sessionStorageService.RemoveItemAsync("UserSession");

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
            }
        }

        public async Task<string> GetToken()
        {
            string result = string.Empty;

            try
            {
                var userSession = await sessionStorageService.ReadEncryptedItemAsync<UserSession>("UserSession");

                if (userSession != null
                    && DateTime.Now < userSession.ExpiryTimeStamp)
                {
                    result = userSession.Token;
                }
            }
            catch
            { }

            return result;
        }
    }
}
