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
    public class EditModel : PageModel
    {
        private readonly AttendanceDbContext _context;

        public EditModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty] public int TeacherId { get; set; }
        [BindProperty] public string FirstName { get; set; } = string.Empty;
        [BindProperty] public string? SecondName { get; set; }
        [BindProperty] public string LastName { get; set; } = string.Empty;
        [BindProperty] public string? SecondSurName { get; set; }
        [BindProperty] public int? DocumentTypeId { get; set; }
        [BindProperty] public string? Document { get; set; }
        [BindProperty] public int? TelephoneCompanyId { get; set; }
        [BindProperty] public string? Phone { get; set; }
        [BindProperty] public string? Email { get; set; }
        [BindProperty] public bool IsActive { get; set; }

        [BindProperty] public bool ManageSystemUser { get; set; }
        [BindProperty] public string? UserName { get; set; }
        [BindProperty] public string? NewPassword { get; set; }
        [BindProperty] public bool UserIsActive { get; set; } = true;

        public bool HasSystemUser { get; set; }
        public string? Message { get; set; }
        public string? SuccessMessage { get; set; }

        public List<SelectListItem> DocumentTypes { get; set; } = new();
        public List<SelectListItem> TelephoneCompanies { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadCatalogsAsync();

            var teacher = await _context.CatTeachers
                .Include(t => t.TblDocumentTeachers)
                .Include(t => t.TblPhoneTeachers)
                .Include(t => t.TblEmailTeachers)
                .FirstOrDefaultAsync(t => t.TeacherId == id);

            if (teacher == null)
                return NotFound();

            LoadTeacherData(teacher);
            await LoadSystemUserAsync(teacher.TeacherId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCatalogsAsync();

            if (TeacherId <= 0)
                return NotFound();

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                Message = "Debe ingresar al menos primer nombre y primer apellido.";
                await LoadSystemUserAsync(TeacherId);
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Document) && DocumentTypeId == null)
            {
                Message = "Debe seleccionar el tipo de documento.";
                await LoadSystemUserAsync(TeacherId);
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Phone) && TelephoneCompanyId == null)
            {
                Message = "Debe seleccionar la compañía telefónica.";
                await LoadSystemUserAsync(TeacherId);
                return Page();
            }

            var cleanDocument = Document?.Trim();
            var cleanPhone = Phone?.Trim();
            var cleanEmail = Email?.Trim().ToLower();
            var cleanUserName = UserName?.Trim();

            var documentExists = !string.IsNullOrWhiteSpace(cleanDocument) &&
                await _context.TblDocumentTeachers
                    .AnyAsync(d => d.TeacherId != TeacherId && d.Document == cleanDocument && d.IsActive);

            if (documentExists)
            {
                Message = "Ya existe otro maestro registrado con ese documento.";
                await LoadSystemUserAsync(TeacherId);
                return Page();
            }

            if (ManageSystemUser)
            {
                if (string.IsNullOrWhiteSpace(cleanUserName))
                {
                    Message = "Debe ingresar un nombre de usuario.";
                    await LoadSystemUserAsync(TeacherId);
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(cleanEmail))
                {
                    Message = "Debe ingresar un correo para crear o actualizar el usuario.";
                    await LoadSystemUserAsync(TeacherId);
                    return Page();
                }

                var existingUserForTeacher = await _context.CatUsers
                    .FirstOrDefaultAsync(u => u.TeacherId == TeacherId);

                var userExists = await _context.CatUsers
                    .AnyAsync(u => u.IdUser != (existingUserForTeacher != null ? existingUserForTeacher.IdUser : 0) &&
                                   (u.UserName.ToLower() == cleanUserName.ToLower() || u.Email.ToLower() == cleanEmail));

                if (userExists)
                {
                    Message = "Ya existe otro usuario con ese nombre de usuario o correo.";
                    await LoadSystemUserAsync(TeacherId);
                    return Page();
                }

                if (existingUserForTeacher == null && string.IsNullOrWhiteSpace(NewPassword))
                {
                    Message = "Debe ingresar una contraseña temporal para crear el usuario.";
                    await LoadSystemUserAsync(TeacherId);
                    return Page();
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var teacher = await _context.CatTeachers
                    .Include(t => t.TblDocumentTeachers)
                    .Include(t => t.TblPhoneTeachers)
                    .Include(t => t.TblEmailTeachers)
                    .FirstOrDefaultAsync(t => t.TeacherId == TeacherId);

                if (teacher == null)
                    return NotFound();

                teacher.FirstName = FirstName.Trim();
                teacher.SecondName = string.IsNullOrWhiteSpace(SecondName) ? null : SecondName.Trim();
                teacher.LastName = LastName.Trim();
                teacher.SecondSurName = string.IsNullOrWhiteSpace(SecondSurName) ? null : SecondSurName.Trim();
                teacher.IsActive = IsActive;

                var currentDocument = teacher.TblDocumentTeachers.FirstOrDefault(d => d.IsActive);
                if (!string.IsNullOrWhiteSpace(cleanDocument) && DocumentTypeId != null)
                {
                    if (currentDocument == null)
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

                var currentPhone = teacher.TblPhoneTeachers.FirstOrDefault(p => p.IsActive);
                if (!string.IsNullOrWhiteSpace(cleanPhone) && TelephoneCompanyId != null)
                {
                    if (currentPhone == null)
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

                var currentEmail = teacher.TblEmailTeachers.FirstOrDefault(e => e.IsActive);
                if (!string.IsNullOrWhiteSpace(cleanEmail))
                {
                    if (currentEmail == null)
                    {
                        _context.TblEmailTeachers.Add(new TblEmailTeacher
                        {
                            TeacherId = teacher.TeacherId,
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

                if (ManageSystemUser)
                {
                    var user = await _context.CatUsers.FirstOrDefaultAsync(u => u.TeacherId == teacher.TeacherId);

                    if (user == null)
                    {
                        var nextUserId = await _context.CatUsers.Select(u => (int?)u.IdUser).MaxAsync() ?? 0;
                        nextUserId++;

                        user = new CatUser
                        {
                            IdUser = nextUserId,
                            TeacherId = teacher.TeacherId,
                            UserName = cleanUserName!,
                            Email = cleanEmail!,
                            PasswordHash = ComputeSha256Hash(NewPassword!.Trim()),
                            IsActive = UserIsActive,
                            CreatedDate = DateTime.Now
                        };

                        _context.CatUsers.Add(user);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        user.UserName = cleanUserName!;
                        user.Email = cleanEmail!;
                        user.IsActive = UserIsActive;

                        if (!string.IsNullOrWhiteSpace(NewPassword))
                            user.PasswordHash = ComputeSha256Hash(NewPassword.Trim());
                    }

                    var teacherRole = await _context.CatRoles
                        .FirstOrDefaultAsync(r => r.RoleName == "TEACHER" && r.IsActive);

                    if (teacherRole == null)
                    {
                        Message = "No existe el rol TEACHER en Cat_Role.";
                        await transaction.RollbackAsync();
                        await LoadSystemUserAsync(TeacherId);
                        return Page();
                    }

                    var hasRole = await _context.TblUserRoles
                        .AnyAsync(r => r.IdUser == user.IdUser && r.IdRole == teacherRole.IdRole && r.IsActive);

                    if (!hasRole)
                    {
                        _context.TblUserRoles.Add(new TblUserRole
                        {
                            IdUser = user.IdUser,
                            IdRole = teacherRole.IdRole,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                SuccessMessage = "Datos del maestro actualizados correctamente.";
                await LoadSystemUserAsync(TeacherId);
                return Page();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Message = "Ocurrió un error al procesar la solicitud. Intenta nuevamente o contacta al administrador.";
                await LoadSystemUserAsync(TeacherId);
                return Page();
            }
        }

        private void LoadTeacherData(CatTeacher teacher)
        {
            TeacherId = teacher.TeacherId;
            FirstName = teacher.FirstName;
            SecondName = teacher.SecondName;
            LastName = teacher.LastName;
            SecondSurName = teacher.SecondSurName;
            IsActive = teacher.IsActive;

            var document = teacher.TblDocumentTeachers.FirstOrDefault(d => d.IsActive);
            if (document != null)
            {
                DocumentTypeId = document.DocumentTypeId;
                Document = document.Document;
            }

            var phone = teacher.TblPhoneTeachers.FirstOrDefault(p => p.IsActive);
            if (phone != null)
            {
                TelephoneCompanyId = phone.IdTelephoneCompany;
                Phone = phone.Phone;
            }

            var email = teacher.TblEmailTeachers.FirstOrDefault(e => e.IsActive);
            if (email != null)
                Email = email.Email;
        }

        private async Task LoadSystemUserAsync(int teacherId)
        {
            var user = await _context.CatUsers.FirstOrDefaultAsync(u => u.TeacherId == teacherId);

            HasSystemUser = user != null;
            if (user == null)
            {
                ManageSystemUser = false;
                UserIsActive = true;
                return;
            }

            ManageSystemUser = true;
            UserName = user.UserName;
            UserIsActive = user.IsActive;
        }

        private async Task LoadCatalogsAsync()
        {
            DocumentTypes = await _context.CatDocumentTypes
                .Where(d => d.IsActive)
                .Select(d => new SelectListItem { Value = d.DocumentTypeId.ToString(), Text = d.DocumentType })
                .ToListAsync();

            TelephoneCompanies = await _context.CatTelephoneCompanies
                .Where(t => t.IsActive)
                .Select(t => new SelectListItem { Value = t.IdTelephoneCompany.ToString(), Text = t.Company })
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
