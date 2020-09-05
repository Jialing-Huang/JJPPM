using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using JJPPM.Data;
using JJPPM.Models;

namespace JJPPM.Pages
{
  [Authorize]
  public class CreateModel : PageModel
  {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    public CreateModel(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
      _db = db;
      _userManager = userManager;
    }

    [BindProperty, Required, MinLength(2), MaxLength(100)]
    public string ProjectName { get; set; }

    [BindProperty, Required, MinLength(2), MaxLength(500)]
    public string Description { get; set; }

    [BindProperty, Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [BindProperty]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime DueDate { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        var newProject = new JProject
        {
          ProjectName = ProjectName,
          Description = Description,
          StartDate = StartDate,
          DueDate = DueDate,
          User = await _userManager.GetUserAsync(User)
        };

        _db.Add(newProject);
        await _db.SaveChangesAsync();  //Database operation as adding new data

        return RedirectToPage("Projects");
      }
      return Page();
    }
  }
}