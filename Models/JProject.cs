using System;
using Microsoft.AspNetCore.Identity;

namespace JJPPM.Models
{
    public class JProject
    {
        public int Id {get; set;}
        public string ProjectName {get; set;}
        public string Description {get; set;}
        public string StartDate {get; set;}
        public string DueDate {get; set;}
        public IdentityUser User {get; set;}
    }
}