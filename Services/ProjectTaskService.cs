using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using JJPPM.Models;
using JJPPM.Data;

namespace JJPPM.Services
{
  public interface IProjectTaskService
  {
    List<JTask> GetTasksByStatus(int projectId, int taskStatusId);
    Task<List<JTask>> GetTasksByStatusAsync(int projectId, int taskStatusId);
    // JH, 2020-09-01
    List<JTaskStatus> GetTaskStatusList();
    List<JTaskPriority> GetTaskPriorityList();
    string GetProjectName(int projectId);
  }

  public class ProjectTaskService : IProjectTaskService
  {
    private readonly ApplicationDbContext _db;
    public ProjectTaskService(ApplicationDbContext db) => _db = db;

    public async Task<List<JTask>> GetTasksByStatusAsync(int projectId, int taskStatusId)
    {
      var project = await _db.Projects.SingleAsync(p => p.Id == projectId);

      var tasks = await _db.Entry(project)
        .Collection(p => p.Tasks)
        .Query()
        .Where(task => task.TaskStatus.Id == taskStatusId)
        .OrderBy(task => task.TaskPriority.Id)
        .ToListAsync();

      return tasks;
    }
    public List<JTask> GetTasksByStatus(int projectId, int taskStatusId)
    {
      var project = _db.Projects.Single(p => p.Id == projectId);

      var tasks = _db.Entry(project)
        .Collection(p => p.Tasks)
        .Query()
        .Where(task => task.TaskStatus.Id == taskStatusId)
        .OrderBy(task => task.TaskPriority.Id)
        .Include(task => task.TaskPriority)
        .Include(task => task.TaskStatus)
        .ToList();

      return tasks;
    }

    public List<JTaskStatus> GetTaskStatusList()
    {
      return _db.TaskStatuses.ToList();
    }

    public List<JTaskPriority> GetTaskPriorityList()
    {
      return _db.TaskPriorities.ToList();
    }

    public string GetProjectName(int projectId)
    {
      return _db.Projects.First(p => p.Id == projectId).ProjectName;
    }
  }
}