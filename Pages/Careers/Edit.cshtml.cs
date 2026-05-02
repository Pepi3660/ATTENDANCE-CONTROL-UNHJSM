using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Careers
{
    [Authorize(Policy = "AdminOnly")]
    public class EditModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public EditModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int CareerId { get; set; }

        [BindProperty]
        public string CareerName { get; set; } = string.Empty;

        [BindProperty]
        public bool IsActive { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var career = await _context.CatCareers.FirstOrDefaultAsync(c => c.CareerId == id);
            if (career == null)
                return NotFound();

            CareerId = career.CareerId;
            CareerName = career.CareerName;
            IsActive = career.IsActive;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(CareerName))
            {
                Message = "Debe ingresar el nombre de la carrera.";
                return Page();
            }

            var cleanName = CareerName.Trim();

            var duplicated = await _context.CatCareers
                .AnyAsync(c => c.CareerId != CareerId && c.CareerName.ToLower() == cleanName.ToLower());

            if (duplicated)
            {
                Message = "Ya existe otra carrera con ese nombre.";
                return Page();
            }

            var career = await _context.CatCareers.FirstOrDefaultAsync(c => c.CareerId == CareerId);
            if (career == null)
                return NotFound();

            career.CareerName = cleanName;
            career.IsActive = IsActive;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
