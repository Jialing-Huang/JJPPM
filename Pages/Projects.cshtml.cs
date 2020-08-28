using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

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
    }
}
