﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EntityWebApi.ViewModels;
using Microsoft.Owin.Security.OAuth;

namespace EntityWebApi.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
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
            if (username.Equals(password)) //реализация для отладки
            {
                return new UserSessionModel
                {
                    UserId = Guid.NewGuid(),
                    Roles = new[] { "User", "Admin" }
                };
            }
            return null;
        }
    }
}