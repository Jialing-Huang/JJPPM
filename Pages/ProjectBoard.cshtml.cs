using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using JJPPM.Models;
using JJPPM.Services;

namespace JJPPM.Pages
{
  public class ProjectBoardModel : PageModel
  {
    private IProjectTaskService _projectTaskService;
    public ProjectBoardModel(IProjectTaskService projectTaskService) => _projectTaskService = projectTaskService;

    public const int TaskStatusCount = 3;

    // public List<JJPPM.Models.Task> Tasks { get; set; }
    // public List<JJPPM.Models.Task> DoingTasks { get; set; }
    // public List<JJPPM.Models.Task> DoneTasks { get; set; }
    public List<JJPPM.Models.Task>[] Tasks { get; set; } = new List<Models.Task>[TaskStatusCount];

    public int ProjectId;

    public void OnGet()
    {
      ProjectId = 1;  // for testing
      for (int i = 0; i < TaskStatusCount; i++)
      {
        Tasks[i] = _projectTaskService.GetTasksByStatus(ProjectId, i + 1);
      }
    }

    public PartialViewResult OnGetProjectBoardPartial(int projectId, int taskStatusId)
    {
      //   Tasks = _projectTaskService.GetTasksByStatus(projectId, taskStatusId);

      //   return new PartialViewResult
      //   {
      //     ViewName = "_ProjectBoardPartial",
      //     ViewData = new ViewDataDictionary<List<JJPPM.Models.Task>>(ViewData, Tasks)
      //   };
      return null;
    }
  }
}
