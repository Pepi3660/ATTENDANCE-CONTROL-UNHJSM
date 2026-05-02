using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Attendance
{
    public class RegisterStudentModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public RegisterStudentModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? Token { get; set; }

        [BindProperty]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string? SecondName { get; set; }

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public string? SecondSurName { get; set; }

        [BindProperty]
        public string Gender { get; set; } = string.Empty;

        [BindProperty]
        public string? Carnet { get; set; }

        [BindProperty]
        public string? Cedula { get; set; }

        [BindProperty]
        public string? Phone { get; set; }

        [BindProperty]
        public int? TelephoneCompanyId { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        public string? Message { get; set; }
        public string MessageType { get; set; } = "info";
        public bool IsSuccess { get; set; }

        public List<SelectListItem> TelephoneCompanies { get; set; } = new();

        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }
        public string? GroupName { get; set; }
        public string? ShiftName { get; set; }
        public string? ClassroomName { get; set; }
        public string? BuildingName { get; set; }
        public string? AreaName { get; set; }
        public string? ClassSchedule { get; set; }
        public string? ClassDateText { get; set; }

        public async Task OnGetAsync(string token, string? value)
        {
            Token = token;
            await LoadCatalogsAsync();
            await LoadClassInfoAsync();

            if (!string.IsNullOrWhiteSpace(value))
            {
                Carnet = value;
                Cedula = value;
                Phone = value;
            }
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
                .FirstOrDefaultAsync(c => c.TokenLink == Token && c.IsActive);

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

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCatalogsAsync();

            if (string.IsNullOrWhiteSpace(Token))
            {
                Message = "El enlace de asistencia no es válido.";
                MessageType = "error";
                return Page();
            }

            await LoadClassInfoAsync();

            if (string.IsNullOrWhiteSpace(Carnet) &&
                string.IsNullOrWhiteSpace(Cedula) &&
                string.IsNullOrWhiteSpace(Phone))
            {
                Message = "Debe ingresar al menos carnet, cédula o celular.";
                MessageType = "warning";
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Phone) && TelephoneCompanyId == null)
            {
                Message = "Debe seleccionar la compañía telefónica.";
                MessageType = "warning";
                return Page();
            }

            var classSession = await _context.TblClasses
                .FirstOrDefaultAsync(c => c.TokenLink == Token && c.IsActive);

            if (classSession == null)
            {
                Message = "La clase no existe o el enlace ya no está disponible.";
                MessageType = "error";
                return Page();
            }

            
            if (classSession.ClassStatusId != 1)
            {
                Message = "La clase no está disponible para asistencia.";
                MessageType = "warning";
                return Page();
            }
var now = DateTime.Now;

            if (classSession.OpenDateTime != null && now < classSession.OpenDateTime)
            {
                Message = "La asistencia aún no está disponible.";
                MessageType = "warning";
                return Page();
            }

            if (classSession.CloseDateTime != null && now > classSession.CloseDateTime)
            {
                Message = "El enlace de asistencia ya expiró.";
                MessageType = "warning";
                return Page();
            }

            var valuesToCheck = new List<string>();

            if (!string.IsNullOrWhiteSpace(Carnet)) valuesToCheck.Add(Carnet.Trim());
            if (!string.IsNullOrWhiteSpace(Cedula)) valuesToCheck.Add(Cedula.Trim());

            var documentExists = await _context.TblDocumentStudents
                .AnyAsync(d => valuesToCheck.Contains(d.Document) && d.IsActive);

            var phoneExists = !string.IsNullOrWhiteSpace(Phone) &&
                await _context.TblPhoneStudents
                    .AnyAsync(p => p.Phone == Phone.Trim() && p.IsActive);

            if (documentExists || phoneExists)
            {
                Message = "Ya existe un estudiante registrado con ese carnet, cédula o celular.";
                MessageType = "warning";
                return Page();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nextStudentId = await _context.CatStudents
                    .Select(s => (int?)s.StudentId)
                    .MaxAsync() ?? 0;

                nextStudentId++;

                var student = new CatStudent
                {
                    StudentId = nextStudentId,
                    FirstName = FirstName.Trim(),
                    SecondName = string.IsNullOrWhiteSpace(SecondName) ? null : SecondName.Trim(),
                    LastName = LastName.Trim(),
                    SecondSurName = string.IsNullOrWhiteSpace(SecondSurName) ? null : SecondSurName.Trim(),
                    Gender = Gender,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _context.CatStudents.Add(student);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(Carnet))
                {
                    _context.TblDocumentStudents.Add(new TblDocumentStudent
                    {
                        StudentId = student.StudentId,
                        DocumentTypeId = 1,
                        Document = Carnet.Trim(),
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrWhiteSpace(Cedula))
                {
                    _context.TblDocumentStudents.Add(new TblDocumentStudent
                    {
                        StudentId = student.StudentId,
                        DocumentTypeId = 2,
                        Document = Cedula.Trim(),
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrWhiteSpace(Phone) && TelephoneCompanyId != null)
                {
                    _context.TblPhoneStudents.Add(new TblPhoneStudent
                    {
                        StudentId = student.StudentId,
                        IdTelephoneCompany = TelephoneCompanyId.Value,
                        Phone = Phone.Trim(),
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrWhiteSpace(Email))
                {
                    _context.TblEmailStudents.Add(new TblEmailStudent
                    {
                        StudentId = student.StudentId,
                        Email = Email.Trim(),
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                _context.TblStudentGroups.Add(new TblStudentGroup
                {
                    StudentId = student.StudentId,
                    GroupId = classSession.GroupId,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });

                _context.TblClassEnrollments.Add(new TblClassEnrollment
                {
                    ClassId = classSession.ClassId,
                    StudentId = student.StudentId,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });

                _context.TblAttendances.Add(new TblAttendance
                {
                    ClassId = classSession.ClassId,
                    StudentId = student.StudentId,
                    AttendanceStatusId = 1,
                    RegisterMethodId = 2,
                    IsJustified = false,
                    RegisterTime = DateTime.Now,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                IsSuccess = true;
                MessageType = "success";
                Message =  $"¡Listo, {student.FirstName} {student.LastName}! Tu registro y asistencia fueron completados.";

                return Page();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageType = "error";
                Message =  $"Ocurrió un error al registrar: {ex.Message}";
                return Page();
            }
        }

        private async Task LoadCatalogsAsync()
        {
            TelephoneCompanies = await _context.CatTelephoneCompanies
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.IdTelephoneCompany.ToString(),
                    Text = c.Company
                })
                .ToListAsync();
        }
    }
}