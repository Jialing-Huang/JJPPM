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
    public DbSet<JTask> Tasks { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // JH, 2020-08-29, Populating data for loopup tables
      builder.Entity<TaskPriority>().HasData(
        new TaskPriority { Id = 1, Name = "HIGH" },
        new TaskPriority { Id = 2, Name = "NORMAL" },
        new TaskPriority { Id = 3, Name = "LOW" }
      );

      builder.Entity<TaskStatus>().HasData(
        new TaskStatus { Id = 1, Name = "TODO" },
        new TaskStatus { Id = 2, Name = "DOING" },
        new TaskStatus { Id = 3, Name = "DONE" }
      );

      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }
  }
}
