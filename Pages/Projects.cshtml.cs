using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using JJPPM.Models;
using JJPPM.Data;

namespace JJPPM.Pages
{
  [Authorize]
  public class ProjectsModel : PageModel
  {
    private readonly ILogger<ProjectsModel> _logger;
    private readonly ApplicationDbContext _db;

    public ProjectsModel(ILogger<ProjectsModel> logger, ApplicationDbContext db)
    {
      _logger = logger;
      _db = db;
    }

    public List<JProject> Projects { get; set; } = new List<JProject>();
    public void OnGet()
    {
      Projects = _db.Projects.ToList();
    }

    public async Task<IActionResult> OnGetDelete(int id)  //How to know the method AND does it connect to delete function
    {
      JProject project = await _db.Projects
        .Include(p => p.Tasks)
        .FirstAsync(p => p.Id == id);

      if (project != null)
      {
        // JH, 2020-09-05, 
        // Need to figure out how to handle cascade deletion in SQLite database
        // Implemented cascading deletion manually
        foreach (var task in project.Tasks)
        {
          _db.Tasks.Remove(task);
        }
        _db.Projects.Remove(project);

        await _db.SaveChangesAsync();
      }

      return RedirectToPage("Projects");
    }
  }
}
