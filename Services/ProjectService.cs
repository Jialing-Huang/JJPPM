using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

using JJPPM.Models;
using JJPPM.Data;

namespace JJPPM.Services
{
  public interface IProjectService
  {
    List<JProject> GetProjectsByPage(ClaimsPrincipal user, int currentPage, int sort, int sortOrder);
    Task<List<JProject>> GetProjectsByPageAsync(ClaimsPrincipal user, int currentPage, int sort, int sortOrder);
    int GetTotalPages(ClaimsPrincipal user);
    int GetProjectsCount(ClaimsPrincipal user);
    Task<bool> RemoveProjectByIdAsync(int id);
  }

  public class ProjectService : IProjectService
  {
    private readonly ApplicationDbContext _db;
    public ProjectService(ApplicationDbContext db) => _db = db;
    readonly int PAGE_SIZE = 3;

    private IQueryable<JProject> SortProjects(ClaimsPrincipal user, int sort, int sortOrder)
    {
      IQueryable<JProject> projects = _db.Projects.Include(p => p.User).Where(p => p.User.UserName == user.Identity.Name);

      switch (sort)
      {
        case 2:
          return sortOrder == 1 ? projects.OrderBy(p => p.DueDate) : projects.OrderByDescending(p => p.DueDate);
        case 3:
          return sortOrder == 1 ? projects.OrderBy(p => p.ProjectName) : projects.OrderByDescending(p => p.ProjectName);
        case 1:
        default:
          return sortOrder == 1 ? projects.OrderBy(p => p.StartDate) : projects.OrderByDescending(p => p.StartDate);
      }
    }

    public async Task<List<JProject>> GetProjectsByPageAsync(ClaimsPrincipal user, int currentPage, int sort = 1, int sortOrder = 1)
    {

      var projects = await SortProjects(user, sort, sortOrder)
          .Skip((currentPage - 1) * PAGE_SIZE)
          .Take(PAGE_SIZE)
          .ToListAsync();

      return projects;
    }

    public List<JProject> GetProjectsByPage(ClaimsPrincipal user, int currentPage, int sort = 1, int sortOrder = 1)
    {
      var projects = SortProjects(user, sort, sortOrder)
          .Skip((currentPage - 1) * PAGE_SIZE)
          .Take(PAGE_SIZE)
          .ToList();

      return projects;
    }

    public int GetTotalPages(ClaimsPrincipal user)
    {
      return (int)Math.Ceiling(decimal.Divide(_db.Projects.Where(p => p.User.UserName == user.Identity.Name).Count(), PAGE_SIZE));
    }

    public int GetProjectsCount(ClaimsPrincipal user)
    {
      return _db.Projects.Where(p => p.User.UserName == user.Identity.Name).Count();
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