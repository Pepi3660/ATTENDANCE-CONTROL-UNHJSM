using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AttendanceControl.Web.Pages.Classes
{
    [Authorize(Policy = "TeacherOrAdmin")]
    public class CreateModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public CreateModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblClass TblClass { get; set; } = new();

        public string? Message { get; set; }

        public bool IsAdmin => User.IsInRole("ADMIN");

        public async Task<IActionResult> OnGetAsync()
        {
            TblClass.ClassDate = DateOnly.FromDateTime(DateTime.Now);
            TblClass.StartTime = TimeOnly.FromDateTime(DateTime.Now);
            TblClass.EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(1));
            TblClass.OpenDateTime = DateTime.Now;
            TblClass.CloseDateTime = DateTime.Now.AddMinutes(10);
            TblClass.AllowSelfRegister = true;

            var openStatus = await _context.CatClassStatuses
                .FirstOrDefaultAsync(x => x.ClassStatusName == "Open" && x.IsActive);

            if (openStatus != null)
                TblClass.ClassStatusId = openStatus.ClassStatusId;

            if (!IsAdmin)
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                if (teacherId == null)
                {
                    Message = "Tu usuario no está asociado a un docente activo. Contacta al administrador.";
                    await LoadSelectListsAsync();
                    return Page();
                }

                TblClass.TeacherId = teacherId.Value;
            }

            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("TblClass.Teacher");
            ModelState.Remove("TblClass.Group");
            ModelState.Remove("TblClass.Subject");
            ModelState.Remove("TblClass.Classroom");
            ModelState.Remove("TblClass.ClassStatus");
            ModelState.Remove("TblClass.TblAttendances");
            ModelState.Remove("TblClass.TblClassEnrollments");

            if (!IsAdmin)
            {
                ModelState.Remove("TblClass.TeacherId");

                var teacherId = await GetCurrentTeacherIdAsync();

                if (teacherId == null)
                {
                    Message = "Tu usuario no está asociado a un docente activo. Contacta al administrador.";
                    await LoadSelectListsAsync();
                    return Page();
                }

                TblClass.TeacherId = teacherId.Value;
            }
            else
            {
                var selectedTeacherIsValid = await _context.CatTeachers
                    .AnyAsync(t =>
                        t.TeacherId == TblClass.TeacherId &&
                        t.IsActive &&
                        t.CatUsers.Any(u => u.IsActive));

                if (!selectedTeacherIsValid)
                    ModelState.AddModelError("TblClass.TeacherId", "Debe seleccionar un docente activo con usuario activo.");
            }

            var selectedGroupIsValid = await _context.CatGroups
                .AnyAsync(g =>
                    g.GroupId == TblClass.GroupId &&
                    g.IsActive &&
                    g.Career.IsActive);

            if (!selectedGroupIsValid)
                ModelState.AddModelError("TblClass.GroupId", "Debe seleccionar un grupo activo asociado a una carrera activa.");

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            TblClass.TokenLink = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "")
                .Substring(0, 16);

            TblClass.IsActive = true;
            TblClass.CreatedDate = DateTime.Now;
            TblClass.AllowSelfRegister = true;

            if (TblClass.OpenDateTime == null)
                TblClass.OpenDateTime = DateTime.Now;

            if (TblClass.CloseDateTime == null)
                TblClass.CloseDateTime = DateTime.Now.AddMinutes(10);

            _context.TblClasses.Add(TblClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task LoadSelectListsAsync()
        {
            ViewData["TeacherId"] = new SelectList(
                await _context.CatTeachers
                    .Where(t => t.IsActive && t.CatUsers.Any(u => u.IsActive))
                    .OrderBy(t => t.FirstName)
                    .Select(t => new
                    {
                        t.TeacherId,
                        FullName = t.FirstName + " " + t.LastName
                    })
                    .ToListAsync(),
                "TeacherId",
                "FullName",
                TblClass.TeacherId
            );

            ViewData["GroupId"] = new SelectList(
                await _context.CatGroups
                    .Include(g => g.Career)
                    .Include(g => g.Shift)
                    .Where(g => g.IsActive && g.Career.IsActive)
                    .OrderBy(g => g.Career.CareerName)
                    .ThenBy(g => g.GroupName)
                    .Select(g => new
                    {
                        g.GroupId,
                        DisplayName = g.Career.CareerName + " - " + g.GroupName + " - " + g.Shift.ShiftName
                    })
                    .ToListAsync(),
                "GroupId",
                "DisplayName",
                TblClass.GroupId
            );

            ViewData["SubjectId"] = new SelectList(
                await _context.CatSubjects
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.SubjectName)
                    .Select(s => new { s.SubjectId, s.SubjectName })
                    .ToListAsync(),
                "SubjectId",
                "SubjectName",
                TblClass.SubjectId
            );

            ViewData["ClassroomId"] = new SelectList(
                await _context.CatClassrooms
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ClassroomName)
                    .Select(c => new { c.ClassroomId, c.ClassroomName })
                    .ToListAsync(),
                "ClassroomId",
                "ClassroomName",
                TblClass.ClassroomId
            );

            ViewData["ClassStatusId"] = new SelectList(
                await _context.CatClassStatuses
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.ClassStatusId)
                    .Select(s => new { s.ClassStatusId, s.ClassStatusName })
                    .ToListAsync(),
                "ClassStatusId",
                "ClassStatusName",
                TblClass.ClassStatusId
            );
        }

        private async Task<int?> GetCurrentTeacherIdAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
                return null;

            return await _context.CatUsers
                .Where(u =>
                    u.IdUser == userId &&
                    u.IsActive &&
                    u.TeacherId > 0 &&
                    u.Teacher.IsActive)
                .Select(u => (int?)u.TeacherId)
                .FirstOrDefaultAsync();
        }
    }
}