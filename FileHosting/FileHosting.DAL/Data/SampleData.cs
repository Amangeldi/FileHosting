using FileHosting.DAL.EF;
using FileHosting.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileHosting.DAL.Data
{
    public class SampleData
    {
        public static async Task Initialize(UserManager<User> userManager, ApiContext context)
        {
            context.Database.EnsureCreated();
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                await userManager.CreateAsync(admin, password);
                FileModel EmptyFile = new FileModel
                {
                    Name = "file.jpg",
                    Path = "/Files/file.jpg",
                    User = admin
                };
                await context.Files.AddAsync(EmptyFile);
                await context.SaveChangesAsync();
            }
        }
    }
}
