using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using JJPPM.Models;
using JJPPM.Data;

namespace JJPPM.Pages
{
    public class ProjectsModel : PageModel
    {
        private readonly ILogger<ProjectsModel> _logger;
        private readonly ApplicationDbContext _db;

        public ProjectsModel(ILogger<ProjectsModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public List<JProject> Projects {get; set;} = new List<JProject>();
        public void OnGet()
        {
            Projects = _db.Projects.ToList();
        }

        public async Task<IActionResult> OnGetDelete(int id)  //How to know the method AND does it connect to delete function
        {
            JProject project = await _db.Projects.FindAsync(id);

            if (project != null)
            {
                _db.Remove(project);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
