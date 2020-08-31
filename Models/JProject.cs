using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace JJPPM.Models
{
  public class JProject
  {
    public int Id { get; set; }
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public IdentityUser User { get; set; }
    public virtual ICollection<Task> Tasks { get; set; }
  }
}