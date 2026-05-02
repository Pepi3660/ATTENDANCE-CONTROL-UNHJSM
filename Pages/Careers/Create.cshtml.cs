using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Careers
{
    [Authorize(Policy = "AdminOnly")]
    public class CreateModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public CreateModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int? CareerId { get; set; }

        [BindProperty]
        public string CareerName { get; set; } = string.Empty;

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(CareerName))
            {
                Message = "Debe ingresar el nombre de la carrera.";
                return Page();
            }

            var cleanName = CareerName.Trim();

            var exists = await _context.CatCareers
                .AnyAsync(c => c.CareerName.ToLower() == cleanName.ToLower());

            if (exists)
            {
                Message = "Ya existe una carrera con ese nombre.";
                return Page();
            }

            var nextId = CareerId;

            if (nextId == null || nextId <= 0)
            {
                nextId = (await _context.CatCareers
                    .Select(c => (int?)c.CareerId)
                    .MaxAsync() ?? 0) + 1;
            }
            else
            {
                var idExists = await _context.CatCareers.AnyAsync(c => c.CareerId == nextId.Value);
                if (idExists)
                {
                    Message = "Ya existe una carrera con ese ID.";
                    return Page();
                }
            }

            _context.CatCareers.Add(new CatCareer
            {
                CareerId = nextId.Value,
                CareerName = cleanName,
                IsActive = true,
                CreatedDate = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
