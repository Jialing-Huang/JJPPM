using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JJPPM.Models;
using JJPPM.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;



namespace JJPPM.Pages
{
    public class TaskUpdateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<IdentityUser> _userManager;
        public TaskUpdateModel(ApplicationDbContext db, UserManager<IdentityUser> userManager)
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

        public async Task<IActionResult> OnGetAsync()
        {
            // JTask task = new JTask();
            JTask task = await _db.Tasks
                .Include(t => t.TaskPriority)
                .Include(t => t.TaskStatus)
                .FirstAsync(t => t.Id == Id);

            Description = task.Description;
            DueDate = task.DueDate;

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


            TaskStatusId = task.TaskStatus.Id;
            TaskPriorityId = task.TaskPriority.Id;

            if (task == null)
            {
                RedirectToPage("Projects");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            /* if (ModelState.IsValid)
            {
                var updatedTask = _db.Tasks.First(f => f.Id == Id);

                updatedTask.Description = Description;
                updatedTask.DueDate = DueDate;
                updatedTask.TaskStatus.Id = TaskStatusId;
                updatedTask.TaskPriority.Id = TaskPriorityId;

                await _db.SaveChangesAsync();

                return RedirectToPage("UpdateSuccess");
            }
            return Page(); */

            if (ModelState.IsValid)
            {
                var updatedTask = new JTask
                {
                    Description = Description,
                    DueDate = DueDate,
                    TaskPriority = await _db.TaskPriorities.FirstAsync(p => p.Id == TaskPriorityId),
                    TaskStatus = await _db.TaskStatuses.FirstAsync(s => s.Id == TaskStatusId),
                };

                await _db.AddAsync(updatedTask);
                await _db.SaveChangesAsync();  //Database operation as adding new data

                return RedirectToPage("ProjectBoard", new { id = Id });
            }
            return Page();
        } 

    }
}
