using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Groups
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
        public int GroupId { get; set; }

        [BindProperty]
        public string GroupName { get; set; } = string.Empty;

        [BindProperty]
        public int CareerId { get; set; }

        [BindProperty]
        public int ShiftId { get; set; }

        [BindProperty]
        public bool IsActive { get; set; }

        public string? Message { get; set; }

        public List<SelectListItem> Careers { get; set; } = new();
        public List<SelectListItem> Shifts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadCatalogsAsync();

            var group = await _context.CatGroups.FirstOrDefaultAsync(g => g.GroupId == id);

            if (group == null)
                return NotFound();

            GroupId = group.GroupId;
            GroupName = group.GroupName;
            CareerId = group.CareerId;
            ShiftId = group.ShiftId;
            IsActive = group.IsActive;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCatalogsAsync();

            if (GroupId <= 0)
                return NotFound();

            if (string.IsNullOrWhiteSpace(GroupName))
            {
                Message = "Debe ingresar el nombre del grupo.";
                return Page();
            }

            var cleanGroupName = GroupName.Trim();

            var exists = await _context.CatGroups.AnyAsync(g =>
                g.GroupId != GroupId &&
                g.GroupName == cleanGroupName &&
                g.CareerId == CareerId &&
                g.ShiftId == ShiftId);

            if (exists)
            {
                Message = "Ya existe otro grupo con ese nombre, carrera y turno.";
                return Page();
            }

            var group = await _context.CatGroups.FirstOrDefaultAsync(g => g.GroupId == GroupId);

            if (group == null)
                return NotFound();

            group.GroupName = cleanGroupName;
            group.CareerId = CareerId;
            group.ShiftId = ShiftId;
            group.IsActive = IsActive;

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