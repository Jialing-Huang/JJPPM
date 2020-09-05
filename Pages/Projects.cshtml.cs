using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using JJPPM.Models;
using JJPPM.Data;
using JJPPM.Services;

namespace JJPPM.Pages
{
  [Authorize]
  public class ProjectsModel : PageModel
  {
    private readonly ILogger<ProjectsModel> _logger;
    // private readonly ApplicationDbContext _db;
    private IProjectService _projectService;
    // public ProjectsModel(ILogger<ProjectsModel> logger, ApplicationDbContext db)
    public ProjectsModel(ILogger<ProjectsModel> logger, IProjectService projectService)
    {
      _logger = logger;
      _projectService = projectService;
    }

    public List<JProject> Projects { get; set; } = new List<JProject>();

    // for paginations
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int Count { get; set; }

    public void OnGet()
    {
      TotalPages = _projectService.GetTotalPages();
      Count = _projectService.GetProjectsCount();
    }

    public PartialViewResult OnGetProjectsPartial(int currentPage, int sort, int sortOrder)
    {
      CurrentPage = currentPage;
      Projects = _projectService.GetProjectsByPage(currentPage, sort, sortOrder);
      return new PartialViewResult
      {
        ViewName = "_ProjectsPartial",
        ViewData = new ViewDataDictionary<List<JProject>>(ViewData, Projects)
      };
    }

    public async Task<IActionResult> OnGetDelete(int id)
    {
      bool result = await _projectService.RemoveProjectByIdAsync(id);

      if (result)
        return RedirectToPage("Projects");

      return Page();
    }
  }
}
