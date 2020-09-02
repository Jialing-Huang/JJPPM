using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JJPPM.Data;
using JJPPM.Models;

namespace JJPPM.Pages
{
  public class CreateModel : PageModel
  {
    private readonly ApplicationDbContext db;
    public CreateModel(ApplicationDbContext db) => this.db = db;


    [BindProperty, Required, MinLength(2), MaxLength(100)]
    public string ProjectName { get; set; }

    [BindProperty, Required, MinLength(2), MaxLength(100)]
    public string Description { get; set; }

    [BindProperty, Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [BindProperty]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:2019-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime DueDate { get; set; }


    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        var newProject = new JProject { ProjectName = ProjectName, Description = Description, StartDate = StartDate, DueDate = DueDate };

        db.Add(newProject);
        await db.SaveChangesAsync();  //Database operation as adding new data

        return RedirectToPage("Projects");
      }
      return Page();
    }
  }
}