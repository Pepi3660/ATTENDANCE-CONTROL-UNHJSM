using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Pages.Students
{
    [Authorize(Policy = "TeacherOrAdmin")]
    public class EditModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public EditModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int StudentId { get; set; }

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
        public int? GroupId { get; set; }

        [BindProperty]
        public bool IsActive { get; set; }

        public string? Message { get; set; }

        public List<SelectListItem> DocumentTypes { get; set; } = new();
        public List<SelectListItem> TelephoneCompanies { get; set; } = new();
        public List<SelectListItem> Groups { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadCatalogsAsync();

            var student = await _context.CatStudents
                .Include(s => s.TblDocumentStudents)
                .Include(s => s.TblPhoneStudents)
                .Include(s => s.TblEmailStudents)
                .Include(s => s.TblStudentGroups)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
                return NotFound();

            StudentId = student.StudentId;
            FirstName = student.FirstName;
            SecondName = student.SecondName;
            LastName = student.LastName;
            SecondSurName = student.SecondSurName;
            Gender = student.Gender;
            IsActive = student.IsActive;

            var document = student.TblDocumentStudents.FirstOrDefault(d => d.IsActive);
            if (document != null)
            {
                DocumentTypeId = document.DocumentTypeId;
                Document = document.Document;
            }

            var phone = student.TblPhoneStudents.FirstOrDefault(p => p.IsActive);
            if (phone != null)
            {
                TelephoneCompanyId = phone.IdTelephoneCompany;
                Phone = phone.Phone;
            }

            var email = student.TblEmailStudents.FirstOrDefault(e => e.IsActive);
            if (email != null)
            {
                Email = email.Email;
            }

            var group = student.TblStudentGroups.FirstOrDefault(g => g.IsActive);
            if (group != null)
            {
                GroupId = group.GroupId;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCatalogsAsync();

            if (StudentId <= 0)
                return NotFound();

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

            var cleanDocument = Document?.Trim();
            var cleanPhone = Phone?.Trim();
            var cleanEmail = Email?.Trim().ToLower();

            var documentExists = !string.IsNullOrWhiteSpace(cleanDocument) &&
                await _context.TblDocumentStudents
                    .AnyAsync(d => d.StudentId != StudentId &&
                                   d.Document == cleanDocument &&
                                   d.IsActive);

            if (documentExists)
            {
                Message = "Ya existe otro estudiante registrado con ese documento.";
                return Page();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var student = await _context.CatStudents
                    .Include(s => s.TblDocumentStudents)
                    .Include(s => s.TblPhoneStudents)
                    .Include(s => s.TblEmailStudents)
                    .Include(s => s.TblStudentGroups)
                    .FirstOrDefaultAsync(s => s.StudentId == StudentId);

                if (student == null)
                    return NotFound();

                student.FirstName = FirstName.Trim();
                student.SecondName = string.IsNullOrWhiteSpace(SecondName) ? null : SecondName.Trim();
                student.LastName = LastName.Trim();
                student.SecondSurName = string.IsNullOrWhiteSpace(SecondSurName) ? null : SecondSurName.Trim();
                student.Gender = Gender;
                student.IsActive = IsActive;

                // Documento
                var currentDocument = student.TblDocumentStudents.FirstOrDefault(d => d.IsActive);

                if (!string.IsNullOrWhiteSpace(cleanDocument) && DocumentTypeId != null)
                {
                    if (currentDocument == null)
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
                    else
                    {
                        currentDocument.DocumentTypeId = DocumentTypeId.Value;
                        currentDocument.Document = cleanDocument;
                    }
                }
                else if (currentDocument != null)
                {
                    currentDocument.IsActive = false;
                }

                // Teléfono
                var currentPhone = student.TblPhoneStudents.FirstOrDefault(p => p.IsActive);

                if (!string.IsNullOrWhiteSpace(cleanPhone) && TelephoneCompanyId != null)
                {
                    if (currentPhone == null)
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
                    else
                    {
                        currentPhone.IdTelephoneCompany = TelephoneCompanyId.Value;
                        currentPhone.Phone = cleanPhone;
                    }
                }
                else if (currentPhone != null)
                {
                    currentPhone.IsActive = false;
                }

                // Email
                var currentEmail = student.TblEmailStudents.FirstOrDefault(e => e.IsActive);

                if (!string.IsNullOrWhiteSpace(cleanEmail))
                {
                    if (currentEmail == null)
                    {
                        _context.TblEmailStudents.Add(new TblEmailStudent
                        {
                            StudentId = student.StudentId,
                            Email = cleanEmail,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        });
                    }
                    else
                    {
                        currentEmail.Email = cleanEmail;
                    }
                }
                else if (currentEmail != null)
                {
                    currentEmail.IsActive = false;
                }

                // Grupo oficial
                var currentGroup = student.TblStudentGroups.FirstOrDefault(g => g.IsActive);

                if (GroupId != null)
                {
                    if (currentGroup == null)
                    {
                        _context.TblStudentGroups.Add(new TblStudentGroup
                        {
                            StudentId = student.StudentId,
                            GroupId = GroupId.Value,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        });
                    }
                    else
                    {
                        currentGroup.GroupId = GroupId.Value;
                    }
                }
                else if (currentGroup != null)
                {
                    currentGroup.IsActive = false;
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
                .Select(d => new SelectListItem
                {
                    Value = d.DocumentTypeId.ToString(),
                    Text = d.DocumentType
                })
                .ToListAsync();

            TelephoneCompanies = await _context.CatTelephoneCompanies
                .Where(t => t.IsActive)
                .Select(t => new SelectListItem
                {
                    Value = t.IdTelephoneCompany.ToString(),
                    Text = t.Company
                })
                .ToListAsync();

            Groups = await _context.CatGroups
                .Include(g => g.Shift)
                .Where(g => g.IsActive)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupName + " - " + g.Shift.ShiftName
                })
                .ToListAsync();
        }
    }
}