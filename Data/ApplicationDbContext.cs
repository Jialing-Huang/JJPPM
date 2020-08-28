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
}
