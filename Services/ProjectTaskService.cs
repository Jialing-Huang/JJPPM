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
    List<JJPPM.Models.Task> GetTasksByStatus(int projectId, int taskStatusId);
    Task<List<JJPPM.Models.Task>> GetTasksByStatusAsync(int projectId, int taskStatusId);
  }

  public class ProjectTaskService : IProjectTaskService
  {
    private readonly ApplicationDbContext _db;
    public ProjectTaskService(ApplicationDbContext db) => _db = db;

    public async Task<List<JJPPM.Models.Task>> GetTasksByStatusAsync(int projectId, int taskStatusId)
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
    public List<JJPPM.Models.Task> GetTasksByStatus(int projectId, int taskStatusId)
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
  }
}