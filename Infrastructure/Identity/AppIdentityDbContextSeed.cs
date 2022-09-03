using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager){
            if(!userManager.Users.Any()){
                var user=new AppUser
                {
                    DisplayName="Bob",
                    Email ="bob@test.com",
                    UserName= "bob@test.com",
                    Address = new Address
                    {
                        FirstName="Bob",
                        LastName= "Bobby",
                        Street ="10 us road",
                        City="NY",
                        ZipCode="23456"
                    }
                };

                await userManager.CreateAsync(user,"M@thu_100");
            }
        }
    }
}