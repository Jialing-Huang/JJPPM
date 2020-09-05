using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using JJPPM.Models;
using JJPPM.Data;

namespace JJPPM.Services
{
  public interface IProjectService
  {
    List<JProject> GetProjectsByPage(int currentPage);
    List<JProject> GetProjectsByPage(int currentPage, int sort);
    Task<List<JProject>> GetProjectsByPageAsync(int currentPage);
    Task<List<JProject>> GetProjectsByPageAsync(int currentPage, int sort);
    int GetTotalPages();
    int GetProjectsCount();
    Task<bool> RemoveProjectByIdAsync(int id);
  }

  public class ProjectService : IProjectService
  {
    private readonly ApplicationDbContext _db;
    public ProjectService(ApplicationDbContext db) => _db = db;
    readonly int PAGE_SIZE = 3;

    public async Task<List<JProject>> GetProjectsByPageAsync(int currentPage)
    {
      var projects = await _db.Projects
          .Skip((currentPage - 1) * PAGE_SIZE)
          .Take(PAGE_SIZE)
          .ToListAsync();

      return projects;
    }

    public async Task<List<JProject>> GetProjectsByPageAsync(int currentPage, int sort)
    {
      var sortedList = _db.Projects
          .OrderBy(p => p.StartDate);

      var projects = await sortedList
          .Skip((currentPage - 1) * PAGE_SIZE)
          .Take(PAGE_SIZE)
          .ToListAsync();

      return projects;
    }

    public List<JProject> GetProjectsByPage(int currentPage)
    {
      var projects = _db.Projects
          .Skip((currentPage - 1) * PAGE_SIZE)
          .Take(PAGE_SIZE)
          .ToList();

      return projects;
    }

    public List<JProject> GetProjectsByPage(int currentPage, int sort)
    {
      IQueryable<JProject> sortedList;
      switch (sort)
      {
        case 2:
          sortedList = _db.Projects.OrderBy(p => p.DueDate);
          break;
        case 3:
          sortedList = _db.Projects.OrderBy(p => p.ProjectName);
          break;
        case 1:
        default:
          sortedList = _db.Projects.OrderBy(p => p.StartDate);
          break;
      }

      var projects = sortedList
          .Skip((currentPage - 1) * PAGE_SIZE)
          .Take(PAGE_SIZE)
          .ToList();

      return projects;
    }

    public int GetTotalPages()
    {
      return (int)Math.Ceiling(decimal.Divide(_db.Projects.Count(), PAGE_SIZE));
    }

    public int GetProjectsCount()
    {
      return _db.Projects.Count();
    }

    public async Task<bool> RemoveProjectByIdAsync(int id)
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

        return true;
      }
      return false;
    }
  }
}