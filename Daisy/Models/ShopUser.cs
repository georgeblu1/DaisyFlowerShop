using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Daisy.Models
{
    public class ShopUser : IdentityUser
    {
        public ShopUser() : base() { }

        public int ContactNumber { get; set; }
    }
}
