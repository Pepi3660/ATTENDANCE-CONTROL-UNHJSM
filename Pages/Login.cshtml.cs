using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AttendanceControl.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AttendanceDbContext _context;

     
        public LoginModel(AttendanceDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var passwordHash = ComputeSha256Hash(Password.Trim());

            var user = await _context.CatUsers
                .FirstOrDefaultAsync(u =>
                    u.IsActive &&
                    (u.UserName == UserNameOrEmail.Trim() || u.Email == UserNameOrEmail.Trim()) &&
                    u.PasswordHash == passwordHash);

            if (user == null)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
          
                return Page();
            }

            var roles = await _context.TblUserRoles
                .Include(ur => ur.IdRoleNavigation)
                .Where(ur => ur.IdUser == user.IdUser && ur.IsActive)
                .Select(ur => ur.IdRoleNavigation.RoleName)
                .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }


    }
}