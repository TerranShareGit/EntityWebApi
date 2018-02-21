﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace EntityWebApi.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public string Description { get; set; }
    }
}