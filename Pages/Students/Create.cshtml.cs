using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Students
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
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string? SecondName { get; set; }

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public string? SecondSurName { get; set; }

        [BindProperty]
        public string Gender { get; set; } = "M";

        [BindProperty]
        public int? DocumentTypeId { get; set; }

        [BindProperty]
        public string? Document { get; set; }

        [BindProperty]
        public int? TelephoneCompanyId { get; set; }

        [BindProperty]
        public string? Phone { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public int? CareerId { get; set; }

        [BindProperty]
        public int? GroupId { get; set; }

        public string? Message { get; set; }

        public List<SelectListItem> DocumentTypes { get; set; } = new();
        public List<SelectListItem> TelephoneCompanies { get; set; } = new();
        public List<SelectListItem> Careers { get; set; } = new();
        public List<SelectListItem> Groups { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadCatalogsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCatalogsAsync();

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                Message = "Debe ingresar al menos primer nombre y primer apellido.";
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Document) && DocumentTypeId == null)
            {
                Message = "Debe seleccionar el tipo de documento.";
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Phone) && TelephoneCompanyId == null)
            {
                Message = "Debe seleccionar la compañía telefónica.";
                return Page();
            }

            if (GroupId != null)
            {
                var groupExists = await _context.CatGroups
                    .AnyAsync(g => g.GroupId == GroupId.Value && g.IsActive);

                if (!groupExists)
                {
                    Message = "El grupo seleccionado no existe o está inactivo.";
                    return Page();
                }
            }

            var cleanDocument = Document?.Trim();
            var cleanPhone = Phone?.Trim();
            var cleanEmail = Email?.Trim().ToLower();

            var documentExists = !string.IsNullOrWhiteSpace(cleanDocument) &&
                await _context.TblDocumentStudents
                    .AnyAsync(d => d.Document == cleanDocument && d.IsActive);

            if (documentExists)
            {
                Message = "Ya existe un estudiante registrado con ese documento.";
                return Page();
            }

            var phoneExists = !string.IsNullOrWhiteSpace(cleanPhone) &&
                await _context.TblPhoneStudents
                    .AnyAsync(p => p.Phone == cleanPhone && p.IsActive);

            if (phoneExists)
            {
                Message = "Ya existe un estudiante registrado con ese teléfono.";
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

                if (!string.IsNullOrWhiteSpace(cleanDocument) && DocumentTypeId != null)
                {
                    _context.TblDocumentStudents.Add(new TblDocumentStudent
                    {
                        StudentId = student.StudentId,
                        DocumentTypeId = DocumentTypeId.Value,
                        Document = cleanDocument,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrWhiteSpace(cleanPhone) && TelephoneCompanyId != null)
                {
                    _context.TblPhoneStudents.Add(new TblPhoneStudent
                    {
                        StudentId = student.StudentId,
                        IdTelephoneCompany = TelephoneCompanyId.Value,
                        Phone = cleanPhone,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrWhiteSpace(cleanEmail))
                {
                    _context.TblEmailStudents.Add(new TblEmailStudent
                    {
                        StudentId = student.StudentId,
                        Email = cleanEmail,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (GroupId != null)
                {
                    _context.TblStudentGroups.Add(new TblStudentGroup
                    {
                        StudentId = student.StudentId,
                        GroupId = GroupId.Value,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Message = "Ocurrió un error al procesar la solicitud. Intenta nuevamente o contacta al administrador.";
                return Page();
            }
        }

        private async Task LoadCatalogsAsync()
        {
            DocumentTypes = await _context.CatDocumentTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DocumentType)
                .Select(d => new SelectListItem
                {
                    Value = d.DocumentTypeId.ToString(),
                    Text = d.DocumentType
                })
                .ToListAsync();

            TelephoneCompanies = await _context.CatTelephoneCompanies
                .Where(t => t.IsActive)
                .OrderBy(t => t.Company)
                .Select(t => new SelectListItem
                {
                    Value = t.IdTelephoneCompany.ToString(),
                    Text = t.Company
                })
                .ToListAsync();

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
                .Include(g => g.Career)
                .Include(g => g.Shift)
                .Where(g => g.IsActive)
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