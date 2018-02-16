using System;

namespace EntityWebApi.ViewModels
{
    public class UserSessionModel
    {
        public Guid UserId { get; set; }
        public string[] Roles { get; set; }
    }
}