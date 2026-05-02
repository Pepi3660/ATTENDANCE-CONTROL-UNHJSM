using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Teachers
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public IndexModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        public IList<CatTeacher> Teachers { get; set; } = new List<CatTeacher>();

        public async Task OnGetAsync()
        {
            var query = _context.CatTeachers
                .Include(t => t.TblDocumentTeachers)
                    .ThenInclude(d => d.DocumentType)
                .Include(t => t.TblPhoneTeachers)
                    .ThenInclude(p => p.IdTelephoneCompanyNavigation)
                .Include(t => t.TblEmailTeachers)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var cleanSearch = Search.Trim().ToLower();

                query = query.Where(t =>
                    t.FirstName.ToLower().Contains(cleanSearch) ||
                    t.LastName.ToLower().Contains(cleanSearch) ||
                    (t.SecondName != null && t.SecondName.ToLower().Contains(cleanSearch)) ||
                    (t.SecondSurName != null && t.SecondSurName.ToLower().Contains(cleanSearch)) ||
                    t.TblDocumentTeachers.Any(d => d.IsActive && d.Document.ToLower().Contains(cleanSearch)) ||
                    t.TblPhoneTeachers.Any(p => p.IsActive && p.Phone.ToLower().Contains(cleanSearch)) ||
                    t.TblEmailTeachers.Any(e => e.IsActive && e.Email.ToLower().Contains(cleanSearch))
                );
            }

            if (Status == "active")
            {
                query = query.Where(t => t.IsActive);
            }
            else if (Status == "inactive")
            {
                query = query.Where(t => !t.IsActive);
            }

            Teachers = await query
                .OrderBy(t => t.FirstName)
                .ThenBy(t => t.LastName)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var teacher = await _context.CatTeachers
                .FirstOrDefaultAsync(t => t.TeacherId == id);

            if (teacher == null)
                return NotFound();

            teacher.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToPage(new
            {
                Search,
                Status
            });
        }
    }
}