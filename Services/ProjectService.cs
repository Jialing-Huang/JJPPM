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
    List<JProject> GetProjectsByPage(int currentPage, int sort, int sortOrder);
    Task<List<JProject>> GetProjectsByPageAsync(int currentPage, int sort, int sortOrder);
    int GetTotalPages();
    int GetProjectsCount();
    Task<bool> RemoveProjectByIdAsync(int id);
  }

  public class ProjectService : IProjectService
  {
    private readonly ApplicationDbContext _db;
    public ProjectService(ApplicationDbContext db) => _db = db;
    readonly int PAGE_SIZE = 3;

    private IQueryable<JProject> SortProjects(int sort, int sortOrder)
    {
      switch (sort)
      {
        case 2:
          return sortOrder == 1 ? _db.Projects.OrderBy(p => p.DueDate) : _db.Projects.OrderByDescending(p => p.DueDate);
        case 3:
          return sortOrder == 1 ? _db.Projects.OrderBy(p => p.ProjectName) : _db.Projects.OrderByDescending(p => p.ProjectName);
        case 1:
        default:
          return sortOrder == 1 ? _db.Projects.OrderBy(p => p.StartDate) : _db.Projects.OrderByDescending(p => p.StartDate);
      }
    }

    public async Task<List<JProject>> GetProjectsByPageAsync(int currentPage, int sort = 1, int sortOrder = 1)
    {

      var projects = await SortProjects(sort, sortOrder)
          .Skip((currentPage - 1) * PAGE_SIZE)
          .Take(PAGE_SIZE)
          .ToListAsync();

      return projects;
    }

    public List<JProject> GetProjectsByPage(int currentPage, int sort = 1, int sortOrder = 1)
    {
      var projects = SortProjects(sort, sortOrder)
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