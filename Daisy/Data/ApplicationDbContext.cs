using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Daisy.Models;

namespace Daisy.Data
{
    public class ApplicationDbContext : IdentityDbContext<ShopUser, ShopRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Daisy.Models.Flower> Flower { get; set; }
        public DbSet<Daisy.Models.Cart> Cart { get; set; }
        public DbSet<Daisy.Models.Order> Order { get; set; }
    }
}
