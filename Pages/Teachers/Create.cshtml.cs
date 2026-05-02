using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AttendanceControl.Web.Pages.Teachers
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
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string? SecondName { get; set; }

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public string? SecondSurName { get; set; }

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
        public bool CreateSystemUser { get; set; }

        [BindProperty]
        public string? UserName { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? Message { get; set; }

        public List<SelectListItem> DocumentTypes { get; set; } = new();
        public List<SelectListItem> TelephoneCompanies { get; set; } = new();

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

            var cleanDocument = Document?.Trim();
            var cleanPhone = Phone?.Trim();
            var cleanEmail = Email?.Trim().ToLower();
            var cleanUserName = UserName?.Trim();

            var documentExists = !string.IsNullOrWhiteSpace(cleanDocument) &&
                await _context.TblDocumentTeachers
                    .AnyAsync(d => d.Document == cleanDocument && d.IsActive);

            if (documentExists)
            {
                Message = "Ya existe un maestro registrado con ese documento.";
                return Page();
            }

            if (CreateSystemUser)
            {
                if (string.IsNullOrWhiteSpace(cleanUserName))
                {
                    Message = "Debe ingresar un nombre de usuario.";
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(cleanEmail))
                {
                    Message = "Debe ingresar un correo para crear el usuario.";
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    Message = "Debe ingresar una contraseña.";
                    return Page();
                }

                var userExists = await _context.CatUsers
                    .AnyAsync(u => u.UserName.ToLower() == cleanUserName.ToLower()
                                || u.Email.ToLower() == cleanEmail);

                if (userExists)
                {
                    Message = "Ya existe un usuario con ese nombre de usuario o correo.";
                    return Page();
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nextTeacherId = await _context.CatTeachers
                    .Select(t => (int?)t.TeacherId)
                    .MaxAsync() ?? 0;

                nextTeacherId++;

                var teacher = new CatTeacher
                {
                    TeacherId = nextTeacherId,
                    FirstName = FirstName.Trim(),
                    SecondName = string.IsNullOrWhiteSpace(SecondName) ? null : SecondName.Trim(),
                    LastName = LastName.Trim(),
                    SecondSurName = string.IsNullOrWhiteSpace(SecondSurName) ? null : SecondSurName.Trim(),
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _context.CatTeachers.Add(teacher);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(cleanDocument) && DocumentTypeId != null)
                {
                    _context.TblDocumentTeachers.Add(new TblDocumentTeacher
                    {
                        TeacherId = teacher.TeacherId,
                        DocumentTypeId = DocumentTypeId.Value,
                        Document = cleanDocument,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrWhiteSpace(cleanPhone) && TelephoneCompanyId != null)
                {
                    _context.TblPhoneTeachers.Add(new TblPhoneTeacher
                    {
                        TeacherId = teacher.TeacherId,
                        IdTelephoneCompany = TelephoneCompanyId.Value,
                        Phone = cleanPhone,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrWhiteSpace(cleanEmail))
                {
                    _context.TblEmailTeachers.Add(new TblEmailTeacher
                    {
                        TeacherId = teacher.TeacherId,
                        Email = cleanEmail,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }

                if (CreateSystemUser)
                {
                    var nextUserId = await _context.CatUsers
                        .Select(u => (int?)u.IdUser)
                        .MaxAsync() ?? 0;

                    nextUserId++;

                    var user = new CatUser
                    {
                        IdUser = nextUserId,
                        TeacherId = teacher.TeacherId,
                        UserName = cleanUserName!,
                        Email = cleanEmail!,
                        PasswordHash = ComputeSha256Hash(Password!.Trim()),
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    _context.CatUsers.Add(user);
                    await _context.SaveChangesAsync();

                    var teacherRole = await _context.CatRoles
                        .FirstOrDefaultAsync(r => r.RoleName == "TEACHER" && r.IsActive);

                    if (teacherRole == null)
                    {
                        Message = "No existe el rol TEACHER en Cat_Role.";
                        await transaction.RollbackAsync();
                        return Page();
                    }

                    _context.TblUserRoles.Add(new TblUserRole
                    {
                        IdUser = user.IdUser,
                        IdRole = teacherRole.IdRole,
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
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}