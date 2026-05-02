using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public IndexModel(AttendanceDbContext context)
        {
            _context = context;
        }

        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public int AttendanceToday { get; set; }

        public async Task OnGetAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            TotalStudents = await _context.CatStudents
                .CountAsync(s => s.IsActive);

            TotalTeachers = await _context.CatTeachers
                .CountAsync(t => t.IsActive);

            TotalClasses = await _context.TblClasses
                .CountAsync(c => c.IsActive);

            AttendanceToday = await _context.TblAttendances
                .Include(a => a.Class)
                .CountAsync(a => a.IsActive && a.Class.ClassDate == today);
        }
    }
}