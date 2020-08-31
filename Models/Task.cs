using System;
using Microsoft.AspNetCore.Identity;

namespace JJPPM.Models
{
  public class Task
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public virtual JProject Project { get; set; }
    // JH, 2020-08-29
    // for future use.
    // sharing document between users
    public IdentityUser User { get; set; }
  }
}