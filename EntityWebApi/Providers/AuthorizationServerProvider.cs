using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EntityWebApi.Models;
using EntityWebApi.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;

namespace EntityWebApi.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private List<ApplicationUser> users = new List<ApplicationUser>()
        {
            new ApplicationUser() {UserName = "admin@mail.com", Password = "Admin-1", IsAdmin = true},
            new ApplicationUser() {UserName = "user@mail.com", Password = "User-1"},
            new ApplicationUser() {UserName = "user2@mail.com", Password = "User-2"},
        };

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var user = Validate(context.UserName, context.Password);

            if (user == null)
            {
                context.Rejected();
                return;
            }

            // create identity
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", user.UserId.ToString())); //user id
            identity.AddClaim(new Claim("role", string.Join(",", user.Roles))); //roles
            context.Validated(identity);
            await Task.FromResult(0);
        }

        private UserSessionModel Validate(string username, string password)
        {
            ApplicationUser user = users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                return new UserSessionModel
                {
                    UserId = Guid.NewGuid(),
                    Roles = user.IsAdmin ? new[] { "Admin" } : new[] { "User" }
                };
            }
            return null;
        }
    }
}