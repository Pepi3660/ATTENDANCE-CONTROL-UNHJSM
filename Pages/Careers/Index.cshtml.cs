using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Careers
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

        public IList<CatCareer> Careers { get; set; } = new List<CatCareer>();

        public async Task OnGetAsync()
        {
            var query = _context.CatCareers
                .Include(c => c.CatGroups)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var clean = Search.Trim().ToLower();
                query = query.Where(c => c.CareerName.ToLower().Contains(clean));
            }

            if (Status == "active")
                query = query.Where(c => c.IsActive);
            else if (Status == "inactive")
                query = query.Where(c => !c.IsActive);

            Careers = await query
                .OrderBy(c => c.CareerName)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var career = await _context.CatCareers.FirstOrDefaultAsync(c => c.CareerId == id);

            if (career == null)
                return NotFound();

            career.IsActive = false;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { Search, Status });
        }
    }
}
