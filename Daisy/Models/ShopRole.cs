using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Daisy.Models
{
    public class ShopRole : IdentityRole
    {
        public ShopRole() : base() { }

        public ShopRole(string rolename) : base(rolename)
        {

        }

        public ShopRole(string rolename, string description, DateTime creationDate) : base(rolename)
        {
            this.Description = description;
            this.CreationDate = creationDate;
        }

        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
