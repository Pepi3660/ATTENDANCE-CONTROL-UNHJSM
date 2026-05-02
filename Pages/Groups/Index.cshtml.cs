using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Groups
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
        public int? CareerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ShiftId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        public IList<CatGroup> Groups { get; set; } = new List<CatGroup>();

        public List<SelectListItem> Careers { get; set; } = new();
        public List<SelectListItem> Shifts { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadCatalogsAsync();

            var query = _context.CatGroups
                .Include(g => g.Career)
                .Include(g => g.Shift)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var clean = Search.Trim().ToLower();

                query = query.Where(g =>
                    g.GroupName.ToLower().Contains(clean) ||
                    g.Career.CareerName.ToLower().Contains(clean) ||
                    g.Shift.ShiftName.ToLower().Contains(clean));
            }

            if (CareerId.HasValue)
                query = query.Where(g => g.CareerId == CareerId.Value);

            if (ShiftId.HasValue)
                query = query.Where(g => g.ShiftId == ShiftId.Value);

            if (Status == "active")
                query = query.Where(g => g.IsActive);
            else if (Status == "inactive")
                query = query.Where(g => !g.IsActive);

            Groups = await query
                .OrderBy(g => g.Career.CareerName)
                .ThenBy(g => g.GroupName)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var group = await _context.CatGroups.FirstOrDefaultAsync(g => g.GroupId == id);

            if (group == null)
                return NotFound();

            group.IsActive = false;
            await _context.SaveChangesAsync();

            return RedirectToPage(new
            {
                Search,
                CareerId,
                ShiftId,
                Status
            });
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

            Shifts = await _context.CatShifts
                .Where(s => s.IsActive)
                .OrderBy(s => s.ShiftName)
                .Select(s => new SelectListItem
                {
                    Value = s.ShiftId.ToString(),
                    Text = s.ShiftName
                })
                .ToListAsync();
        }
    }
}