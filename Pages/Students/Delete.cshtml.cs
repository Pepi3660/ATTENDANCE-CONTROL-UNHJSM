using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AttendanceControl.Web.Models;

namespace AttendanceControl.Web.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly AttendanceControl.Web.Models.AttendanceDbContext _context;

        public DeleteModel(AttendanceControl.Web.Models.AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CatStudent CatStudent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catstudent = await _context.CatStudents.FirstOrDefaultAsync(m => m.StudentId == id);

            if (catstudent == null)
            {
                return NotFound();
            }
            else
            {
                CatStudent = catstudent;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catstudent = await _context.CatStudents.FindAsync(id);
            if (catstudent != null)
            {
                CatStudent = catstudent;
                _context.CatStudents.Remove(CatStudent);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
