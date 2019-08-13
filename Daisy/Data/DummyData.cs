using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Daisy.Models;

namespace Daisy.Data
{
    public class DummyData
    {
        public static async Task Initialize(ApplicationDbContext context,
                              UserManager<ShopUser> userManager,
                              RoleManager<ShopRole> roleManager)
        {
            context.Database.EnsureCreated();

            String adminId1 = "";

            string role1 = "Admin";
            string desc1 = "This is the administrator role";

            string role2 = "Member";
            string desc2 = "This is the members role";

            string password = "P@$$w0rd";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ShopRole(role1, desc1, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ShopRole(role2, desc2, DateTime.Now));
            }

            if (await userManager.FindByNameAsync("aa@aa.aa") == null)
            {
                var user = new ShopUser
                {
                    UserName = "aa@aa.aa",
                    Email = "aa@aa.aa",
                    PhoneNumber = "0188888888"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId1 = user.Id;
            }

            if (await userManager.FindByNameAsync("bb@bb.bb") == null)
            {
                var user = new ShopUser
                {
                    UserName = "bb@bb.bb",
                    Email = "bb@bb.bb",
                    PhoneNumber = "0188888888"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }


        }
    }
}
