﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text.Json;

using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Data
{
    public class Seed
    {

        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            {
                if (await userManager.Users.AnyAsync()) return;

                var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
                if (users == null) return;

                var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    user.UserName = user.UserName.ToLower();
                    await userManager.CreateAsync(user, "Pass123");
                    await userManager.AddToRoleAsync(user, "Member");
                }

                var admin = new AppUser
                {
                    UserName = "admin"
                };

                await userManager.CreateAsync(admin, "Pass123");
                await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
            }
        }
    }
}