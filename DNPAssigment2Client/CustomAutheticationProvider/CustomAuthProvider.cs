using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace DNPAssigment1.Login
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IUserService _userService;
        private User _cachedUser;

        public CustomAuthProvider(IJSRuntime jsRuntime, IUserService userService)
        {
            _jsRuntime = jsRuntime;
            _userService = userService;
        }

        public User GetCachedUser()
        {
            return _cachedUser;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (_cachedUser == null)
            {
                var userAsJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    var tmp = JsonSerializer.Deserialize<User>(userAsJson);
                    ValidateLogin(tmp.Username, tmp.Password);
                }
            }
            else
            {
                identity = SetupClaimsForUser(_cachedUser);
            }

            var cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        private ClaimsIdentity SetupClaimsForUser(User cachedUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cachedUser.Username),
                new Claim("Level", cachedUser.SecurityLvl.ToString())
            };

            var identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }

        public async Task ValidateLogin(string tmpUsername, string tmpPassword)
        {
            if (string.IsNullOrEmpty(tmpUsername)) throw new ArithmeticException("No username specified");
            if (string.IsNullOrEmpty(tmpPassword)) throw new ArithmeticException("No password specified");

            ClaimsIdentity identity = new ClaimsIdentity();

            try
            {

                var user = await _userService.ValidateUser(tmpUsername, tmpPassword);

                identity = SetupClaimsForUser(user);

                string serializedData = JsonSerializer.Serialize(user);

                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serializedData);

                _cachedUser = user;

            }
            catch(Exception e)
            {
                throw e;

            }

            NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
            
        }

        public void Logout()
        {
            _cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}