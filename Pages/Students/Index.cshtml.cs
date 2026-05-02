using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Students
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
        public int? CareerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? GroupId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Gender { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Status { get; set; } = "active";

        public IList<CatStudent> Students { get; set; } = new List<CatStudent>();
        public List<SelectListItem> Careers { get; set; } = new();
        public List<SelectListItem> Groups { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadCatalogsAsync();

            var query = _context.CatStudents
                .Include(s => s.TblDocumentStudents)
                    .ThenInclude(d => d.DocumentType)
                .Include(s => s.TblPhoneStudents)
                    .ThenInclude(p => p.IdTelephoneCompanyNavigation)
                .Include(s => s.TblEmailStudents)
                .Include(s => s.TblStudentGroups)
                    .ThenInclude(g => g.Group)
                        .ThenInclude(g => g.Shift)
                .Include(s => s.TblStudentGroups)
                    .ThenInclude(g => g.Group)
                        .ThenInclude(g => g.Career)
                .AsQueryable();

            if (Status == "active")
                query = query.Where(s => s.IsActive);
            else if (Status == "inactive")
                query = query.Where(s => !s.IsActive);

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var cleanSearch = Search.Trim().ToLower();

                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(cleanSearch) ||
                    s.LastName.ToLower().Contains(cleanSearch) ||
                    (s.SecondName != null && s.SecondName.ToLower().Contains(cleanSearch)) ||
                    (s.SecondSurName != null && s.SecondSurName.ToLower().Contains(cleanSearch)) ||
                    s.TblDocumentStudents.Any(d => d.IsActive && d.Document.ToLower().Contains(cleanSearch)) ||
                    s.TblPhoneStudents.Any(p => p.IsActive && p.Phone.ToLower().Contains(cleanSearch)) ||
                    s.TblEmailStudents.Any(e => e.IsActive && e.Email.ToLower().Contains(cleanSearch))
                );
            }

            if (CareerId.HasValue)
            {
                query = query.Where(s => s.TblStudentGroups.Any(g =>
                    g.IsActive && g.Group.CareerId == CareerId.Value));
            }

            if (GroupId.HasValue)
            {
                query = query.Where(s => s.TblStudentGroups.Any(g =>
                    g.IsActive && g.GroupId == GroupId.Value));
            }

            if (!string.IsNullOrWhiteSpace(Gender))
                query = query.Where(s => s.Gender == Gender);

            Students = await query
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var student = await _context.CatStudents.FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
                return NotFound();

            student.IsActive = false;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { Search, CareerId, GroupId, Gender, Status });
        }

        private async Task LoadCatalogsAsync()
        {
            Careers = await _context.CatCareers
                .Where(c => c.IsActive)
                .OrderBy(c => c.CareerName)
                .Select(c => new SelectListItem
                {
                    Value = c.CareerId.ToString(),
                    Text = c.CareerName
                })
                .ToListAsync();

            Groups = await _context.CatGroups
                .Include(g => g.Shift)
                .Include(g => g.Career)
                .Where(g => g.IsActive && g.Career.IsActive)
                .OrderBy(g => g.Career.CareerName)
                .ThenBy(g => g.GroupName)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupName
                    //Text = g.Career.CareerName + " - " + g.GroupName + " - " + g.Shift.ShiftName
                })
                .ToListAsync();
        }
    }
}
