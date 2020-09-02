using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using JJPPM.Models;
using JJPPM.Services;

namespace JJPPM.Pages
{
  [Authorize]
  public class ProjectBoardModel : PageModel
  {
    private IProjectTaskService _projectTaskService;
    public ProjectBoardModel(IProjectTaskService projectTaskService) => _projectTaskService = projectTaskService;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }   // ProjectId
    public string ProjectName { get; set; }

    public List<JTaskPriority> TaskPriorities { get; set; }
    public List<JTaskStatus> TaskStatuses { get; set; }

    public List<JTask>[] Tasks { get; set; }

    public void OnGet()
    {
      ProjectName = _projectTaskService.GetProjectName(Id);
      TaskPriorities = _projectTaskService.GetTaskPriorityList();
      TaskStatuses = _projectTaskService.GetTaskStatusList();

      Tasks = new List<JTask>[TaskStatuses.Count];

      for (int i = 0; i < TaskStatuses.Count; i++)
      {
        Tasks[i] = _projectTaskService.GetTasksByStatus(Id, i + 1);
      }
    }
  }
}
