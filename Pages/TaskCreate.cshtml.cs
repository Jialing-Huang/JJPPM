using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

using JJPPM.Data;
using JJPPM.Models;


namespace JJPPM.Pages
{
  public class TaskCreateModel : PageModel
  {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    public TaskCreateModel(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
      _db = db;
      _userManager = userManager;
    }

    [BindProperty, Required, MinLength(2), MaxLength(100)]
    public string Description { get; set; }

    [BindProperty, Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime DueDate { get; set; } = DateTime.Now;

    [BindProperty]
    public int TaskStatusId { get; set; }
    [BindProperty]
    public int TaskPriorityId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public List<SelectListItem> TaskStatusOptions { get; set; }
    public List<SelectListItem> TaskPriorityOptions { get; set; }

    public void OnGet()
    {
      TaskStatusOptions = _db.TaskStatuses.Select(s =>
          new SelectListItem
          {
            Value = s.Id.ToString(),
            Text = s.Name
          }).ToList();

      TaskPriorityOptions = _db.TaskPriorities.Select(p =>
          new SelectListItem
          {
            Value = p.Id.ToString(),
            Text = p.Name
          }).ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        var newTask = new JTask
        {
          Description = Description,
          DueDate = DueDate,
          TaskPriority = await _db.TaskPriorities.FirstAsync(p => p.Id == TaskPriorityId),
          TaskStatus = await _db.TaskStatuses.FirstAsync(s => s.Id == TaskStatusId),
          User = await _userManager.GetUserAsync(User),
          Project = await _db.Projects.FirstAsync(p => p.Id == Id)
        };

        await _db.AddAsync(newTask);
        await _db.SaveChangesAsync();  //Database operation as adding new data

        return RedirectToPage("ProjectBoard", new { id = Id });
      }
      return Page();
    }
  }
}
