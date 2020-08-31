using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using JJPPM.Models;
using JJPPM.Data;

namespace JJPPM.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public UpdateModel(ApplicationDbContext db) => this.db = db;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty, Required, MinLength(2), MaxLength(100)]
        public string ProjectName { get; set; }

        [BindProperty, Required, MinLength(2), MaxLength(100)]
        public string Description { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime DueDate { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            JProject project = await db.Projects.FindAsync(Id);

            if (project == null)
            {
                return RedirectToPage("Projects");
            }
        
            ProjectName = project.ProjectName;
            Description = project.Description;
            StartDate = project.StartDate;
            DueDate = project.DueDate;
           
            return Page();
        } 

         public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var project = db.Projects.First(f => f.Id == Id);

                project.ProjectName = ProjectName;
                project.Description = Description;
                project.StartDate = StartDate;
                project.DueDate = DueDate;

                await db.SaveChangesAsync();

                return RedirectToPage("UpdateSuccess");
            }
            return Page();
        }
    }
}
