using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AttendanceControl.Web.Models;

namespace AttendanceControl.Web.Pages.Classes
{
    public class DetailsModel : PageModel
    {
        private readonly AttendanceControl.Web.Models.AttendanceDbContext _context;

        public DetailsModel(AttendanceControl.Web.Models.AttendanceDbContext context)
        {
            _context = context;
        }

        public TblClass TblClass { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblclass = await _context.TblClasses.FirstOrDefaultAsync(m => m.ClassId == id);
            if (tblclass == null)
            {
                return NotFound();
            }
            else
            {
                TblClass = tblclass;
            }
            return Page();
        }
    }
}
