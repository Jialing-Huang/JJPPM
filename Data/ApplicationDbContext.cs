using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using JJPPM.Models;

namespace JJPPM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<JProject> Projects { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

    // public class ProjectContext : DbContext
    // {
    //     public DbSet<Project> Projects { get; set; }

    //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     {
    //         optionsBuilder.UseSqlite(@"Data source=app.db");
    //     }
    // }
}
