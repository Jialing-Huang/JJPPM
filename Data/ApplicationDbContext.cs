using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using JJPPM.Models;

namespace JJPPM.Data
{
  public class ApplicationDbContext : IdentityDbContext
  {
    public DbSet<JProject> Projects { get; set; }
    public DbSet<JTask> Tasks { get; set; }
    // JH, 2020-09-01
    public DbSet<JTaskPriority> TaskPriorities { get; set; }
    public DbSet<JTaskStatus> TaskStatuses { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // JH, 2020-08-29, Populating data for loopup tables
      builder.Entity<JTaskPriority>().HasData(
        new JTaskPriority { Id = 1, Name = "LOW" },
        new JTaskPriority { Id = 2, Name = "NORMAL" },
        new JTaskPriority { Id = 3, Name = "HIGH" }
      );

      builder.Entity<JTaskStatus>().HasData(
        new JTaskStatus { Id = 1, Name = "TO-DO" },
        new JTaskStatus { Id = 2, Name = "IN PROGRESS" },
        new JTaskStatus { Id = 3, Name = "COMPLETED" }
      );

      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }
  }
}
