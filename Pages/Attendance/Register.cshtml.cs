using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Attendance
{
    public class RegisterModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public RegisterModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? Token { get; set; }

        [BindProperty]
        public string SearchValue { get; set; } = string.Empty;

        public string? Message { get; set; }
        public string MessageType { get; set; } = "info";
        public bool ShowRegisterButton { get; set; }
        public bool IsSuccess { get; set; }

        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }
        public string? GroupName { get; set; }
        public string? ShiftName { get; set; }
        public string? ClassroomName { get; set; }
        public string? BuildingName { get; set; }
        public string? AreaName { get; set; }
        public string? ClassSchedule { get; set; }
        public string? ClassDateText { get; set; }

        public async Task OnGetAsync(string token)
        {
            Token = token;
            await LoadClassInfoAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadClassInfoAsync();

            if (string.IsNullOrWhiteSpace(Token))
            {
                SetMessage("El enlace de asistencia no es válido.", "error");
                return Page();
            }

            var classSession = await _context.TblClasses
                .FirstOrDefaultAsync(c => c.TokenLink == Token && c.IsActive);

            if (classSession == null)
            {
                SetMessage("La clase no existe o el enlace ya no está disponible.", "error");
                return Page();
            }

            if (classSession.ClassStatusId != 1)
            {
                SetMessage("La clase no está disponible para asistencia.", "warning");
                return Page();
            }

            var now = DateTime.Now;

            if (classSession.OpenDateTime != null && now < classSession.OpenDateTime)
            {
                SetMessage("La asistencia aún no está disponible.", "warning");
                return Page();
            }

            if (classSession.CloseDateTime != null && now > classSession.CloseDateTime)
            {
                classSession.ClassStatusId = 2; // Closed
                await _context.SaveChangesAsync();

                SetMessage("El enlace de asistencia ya expiró.", "warning");
                return Page();
            }

            var value = SearchValue.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                SetMessage("Debes ingresar carnet, código persona, cédula o celular.", "warning");
                return Page();
            }

            var studentByDocument = await _context.TblDocumentStudents
                .Include(d => d.Student)
                .FirstOrDefaultAsync(d => d.Document == value && d.IsActive);

            var studentByPhone = await _context.TblPhoneStudents
                .Include(p => p.Student)
                .FirstOrDefaultAsync(p => p.Phone == value && p.IsActive);

            var student = studentByDocument?.Student ?? studentByPhone?.Student;

            if (student == null)
            {
                ShowRegisterButton = true;
                SetMessage("No encontramos tus datos. Puedes registrarte por primera vez para esta clase.", "info");
                return Page();
            }

            if (!student.IsActive)
            {
                SetMessage("Tu registro de estudiante está inactivo. Debes comunicarte con administración antes de marcar asistencia.", "error");
                return Page();
            }

            var alreadyRegistered = await _context.TblAttendances
                .AnyAsync(a =>
                    a.ClassId == classSession.ClassId &&
                    a.StudentId == student.StudentId &&
                    a.IsActive);

            if (alreadyRegistered)
            {
                IsSuccess = true;
                SetMessage($"Ya habías registrado asistencia, {student.FirstName} {student.LastName}.", "success");
                return Page();
            }

            var enrollmentExists = await _context.TblClassEnrollments
                .AnyAsync(e =>
                    e.ClassId == classSession.ClassId &&
                    e.StudentId == student.StudentId &&
                    e.IsActive);

            if (!enrollmentExists)
            {
                _context.TblClassEnrollments.Add(new TblClassEnrollment
                {
                    ClassId = classSession.ClassId,
                    StudentId = student.StudentId,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });
            }

            var attendanceStatusId = GetAttendanceStatusId(classSession, now);

            _context.TblAttendances.Add(new TblAttendance
            {
                ClassId = classSession.ClassId,
                StudentId = student.StudentId,
                AttendanceStatusId = attendanceStatusId,
                RegisterMethodId = 2,
                IsJustified = false,
                RegisterTime = now,
                IsActive = true,
                CreatedDate = now
            });

            await _context.SaveChangesAsync();

            IsSuccess = true;
            SetMessage($"¡Listo, {student.FirstName} {student.LastName}! Tu asistencia ha sido registrada.", "success");

            return Page();
        }

        private int GetAttendanceStatusId(TblClass classSession, DateTime now)
        {
            const int present = 1;
            const int late = 2;
            const int toleranceMinutes = 10;

            if (classSession.StartTime == null)
                return present;

            var classStartDateTime = classSession.ClassDate.ToDateTime(classSession.StartTime.Value);
            var lateLimit = classStartDateTime.AddMinutes(toleranceMinutes);

            if (now > classStartDateTime && now <= lateLimit)
                return late;

            return present;
        }

        private async Task<bool> LoadClassInfoAsync()
        {
            if (string.IsNullOrWhiteSpace(Token))
                return false;

            var classInfo = await _context.TblClasses
                .Include(c => c.Teacher)
                .Include(c => c.Subject)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Shift)
                .Include(c => c.Classroom)
                    .ThenInclude(cr => cr.Building)
                        .ThenInclude(b => b.Area)
                .FirstOrDefaultAsync(c => c.TokenLink == Token);

            if (classInfo == null)
                return false;

            SubjectName = classInfo.Subject.SubjectName;
            TeacherName = $"{classInfo.Teacher.FirstName} {classInfo.Teacher.LastName}";
            GroupName = classInfo.Group.GroupName;
            ShiftName = classInfo.Group.Shift.ShiftName;
            ClassroomName = classInfo.Classroom.ClassroomName;
            BuildingName = classInfo.Classroom.Building.BuildingName;
            AreaName = classInfo.Classroom.Building.Area.Initial;
            ClassDateText = classInfo.ClassDate.ToString("dd/MM/yyyy");
            ClassSchedule = $"{classInfo.StartTime:HH\\:mm} - {classInfo.EndTime:HH\\:mm}";

            return true;
        }

        private void SetMessage(string message, string type)
        {
            Message = message;
            MessageType = type;
        }
    }
}