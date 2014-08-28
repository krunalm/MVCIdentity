using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCIdentity.Models
{
    public class MyIdentityRole : IdentityRole
    {
        public MyIdentityRole()
        {

        }

        public MyIdentityRole(string role, string description)
            : base(role)
        {
            this.Description = description;
        }

        public string Description { get; set; }
    }
}