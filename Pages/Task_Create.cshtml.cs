using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JJPPM.Data;
using JJPPM.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JJPPM.Pages
{
    public class Task_CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public Task_CreateModel(ApplicationDbContext db) => this.db = db;
       
        public Task_CreateModel(string description, DateTime dueDate, Models.TaskStatus taskStatus)
        {
            this.Description = description;
            this.DueDate = dueDate;
            // this.TaskStatus.Name = taskStatus.Name;
        }

        [BindProperty, Required, MinLength(2), MaxLength(100)]
        public string Description { get; set; }

        [BindProperty, Required]
        public DateTime DueDate { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var newTask = new JTask { Description = Description, DueDate = DueDate};

            db.Add(newTask);
            await db.SaveChangesAsync();  //Database operation as adding new data

            return RedirectToPage("Projects");
        }
        return Page();
    }

       /*  public override bool Equals(object obj)
        {
            return obj is Task_CreateModel model &&
                   EqualityComparer<Models.TaskStatus>.Default.Equals(taskStatus, model.taskStatus);
        } */
    }
}
