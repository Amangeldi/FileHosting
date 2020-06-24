using FileHosting.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileHosting.DAL.EF
{
    public class ApiContext : IdentityDbContext<User>
    {
        public DbSet<FileModel> Files { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}
