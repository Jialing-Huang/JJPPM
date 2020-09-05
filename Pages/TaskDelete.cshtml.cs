using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using JJPPM.Models;
using JJPPM.Data;

namespace JJPPM.Pages
{
    public class TaskDeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public TaskDeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public List<JTask> Tasks { get; set; } = new List<JTask>();
         public void OnGet()
        {
            Tasks = _db.Tasks.ToList();
        } 
        public async Task<IActionResult> OnGetDelete(int Id) 
        {
            /* JTask task = await _db.Tasks
                .Include(t => t.TaskPriority)
                .Include(t => t.TaskStatus)
                .FirstAsync(t => t.Id == Id); */

                JTask task =  _db.Tasks.Find(Id);

            if (task != null)
            {
                _db.Remove(task);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage("Projects");
        }
    }
}
