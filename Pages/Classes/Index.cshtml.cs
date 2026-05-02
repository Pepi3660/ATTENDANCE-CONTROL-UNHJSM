using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AttendanceControl.Web.Pages.Classes
{
    [Authorize(Policy = "TeacherOrAdmin")]
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
        public DateTime? Date { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CareerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? TeacherId { get; set; }

        public IList<TblClass> Classes { get; set; } = new List<TblClass>();
        public List<CatTeacher> Teachers { get; set; } = new();
        public List<CatCareer> Careers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("ADMIN");

            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            var user = await _context.CatUsers
                .Include(u => u.Teacher)
                .FirstOrDefaultAsync(u => u.IdUser == userId);

            if (user == null || !user.IsActive || (!isAdmin && !user.Teacher.IsActive))
            {
                Classes = new List<TblClass>();
                return Page();
            }

            var query = _context.TblClasses
                .Include(c => c.Teacher)
                .Include(c => c.Subject)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Shift)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Career)
                .Include(c => c.Classroom)
                .Include(c => c.ClassStatus)
                .Where(c => c.IsActive)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(c => c.TeacherId == user.TeacherId);

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var clean = Search.Trim().ToLower();

                query = query.Where(c =>
                    c.Subject.SubjectName.ToLower().Contains(clean) ||
                    c.Group.GroupName.ToLower().Contains(clean) ||
                    c.Group.Career.CareerName.ToLower().Contains(clean) ||
                    c.Classroom.ClassroomName.ToLower().Contains(clean) ||
                    c.Teacher.FirstName.ToLower().Contains(clean) ||
                    c.Teacher.LastName.ToLower().Contains(clean)
                );
            }

            if (Date.HasValue)
            {
                var dateOnly = DateOnly.FromDateTime(Date.Value);
                query = query.Where(c => c.ClassDate == dateOnly);
            }

            if (CareerId.HasValue)
                query = query.Where(c => c.Group.CareerId == CareerId.Value);

            if (isAdmin && TeacherId.HasValue)
                query = query.Where(c => c.TeacherId == TeacherId.Value);

            Classes = await query
                .OrderByDescending(c => c.ClassDate)
                .ThenBy(c => c.StartTime)
                .ToListAsync();

            Careers = await _context.CatCareers
                .Where(c => c.IsActive)
                .OrderBy(c => c.CareerName)
                .ToListAsync();

            if (isAdmin)
            {
                Teachers = await _context.CatTeachers
                    .Where(t => t.IsActive && t.CatUsers.Any(u => u.IsActive))
                    .OrderBy(t => t.FirstName)
                    .ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("ADMIN");

            if (!int.TryParse(userIdValue, out var userId))
                return Forbid();

            var classSession = await _context.TblClasses.FirstOrDefaultAsync(c => c.ClassId == id);
            if (classSession == null)
                return NotFound();

            if (!isAdmin)
            {
                var teacherId = await _context.CatUsers
                    .Where(u => u.IdUser == userId && u.IsActive && u.Teacher.IsActive)
                    .Select(u => (int?)u.TeacherId)
                    .FirstOrDefaultAsync();

                if (teacherId == null || classSession.TeacherId != teacherId.Value)
                    return Forbid();
            }

            classSession.IsActive = false;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { Search, Date, CareerId, TeacherId });
        }
    }
}
