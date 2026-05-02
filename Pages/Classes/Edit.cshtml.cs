using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttendanceControl.Web.Models;

namespace AttendanceControl.Web.Pages.Classes
{
    public class EditModel : PageModel
    {
        private readonly AttendanceControl.Web.Models.AttendanceDbContext _context;

        public EditModel(AttendanceControl.Web.Models.AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblClass TblClass { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblclass =  await _context.TblClasses.FirstOrDefaultAsync(m => m.ClassId == id);
            if (tblclass == null)
            {
                return NotFound();
            }
            TblClass = tblclass;
           ViewData["ClassStatusId"] = new SelectList(_context.CatClassStatuses, "ClassStatusId", "ClassStatusId");
           ViewData["ClassroomId"] = new SelectList(_context.CatClassrooms, "ClassroomId", "ClassroomId");
           ViewData["GroupId"] = new SelectList(_context.CatGroups, "GroupId", "GroupId");
           ViewData["SubjectId"] = new SelectList(_context.CatSubjects, "SubjectId", "SubjectId");
           ViewData["TeacherId"] = new SelectList(_context.CatTeachers, "TeacherId", "TeacherId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TblClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblClassExists(TblClass.ClassId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TblClassExists(int id)
        {
            return _context.TblClasses.Any(e => e.ClassId == id);
        }
    }
}
