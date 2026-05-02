using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Groups
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
        public string GroupName { get; set; } = string.Empty;

        [BindProperty]
        public int CareerId { get; set; }

        [BindProperty]
        public int ShiftId { get; set; }

        public string? Message { get; set; }

        public List<SelectListItem> Careers { get; set; } = new();
        public List<SelectListItem> Shifts { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadCatalogsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCatalogsAsync();

            if (string.IsNullOrWhiteSpace(GroupName))
            {
                Message = "Debe ingresar el nombre del grupo.";
                return Page();
            }

            if (CareerId <= 0)
            {
                Message = "Debe seleccionar una carrera.";
                return Page();
            }

            if (ShiftId <= 0)
            {
                Message = "Debe seleccionar un turno.";
                return Page();
            }

            var cleanGroupName = GroupName.Trim();

            var exists = await _context.CatGroups.AnyAsync(g =>
                g.GroupName == cleanGroupName &&
                g.CareerId == CareerId &&
                g.ShiftId == ShiftId);

            if (exists)
            {
                Message = "Ya existe un grupo con ese nombre, carrera y turno.";
                return Page();
            }

            var nextGroupId = await _context.CatGroups
                .Select(g => (int?)g.GroupId)
                .MaxAsync() ?? 0;

            nextGroupId++;

            var group = new CatGroup
            {
                GroupId = nextGroupId,
                GroupName = cleanGroupName,
                CareerId = CareerId,
                ShiftId = ShiftId,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _context.CatGroups.Add(group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
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