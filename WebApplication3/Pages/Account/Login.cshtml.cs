using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc; // ����������� ������������ ���� ��� ������ � MVC
using Microsoft.AspNetCore.Mvc.RazorPages; // ����������� ��� ������ � Razor Pages
using Microsoft.EntityFrameworkCore; // ����������� ��� ������ � EF Core
using System; // �������� ���� � �������
using System.Collections.Generic; // �������������� ����������� ��� IList
using System.ComponentModel.DataAnnotations; // �������������� ��������� ������
using System.Threading.Tasks; // ����������� ��� ����������� ��������
using WebApplication3.Model.UserAccount; // ����������� ������ ���������������� ������� ������

namespace WebApplication3.Pages.Account // ������������ ���� ��� ������� ��������
{
    public class LoginModel : PageModel // ������ �������� ��� �����
    {
        private readonly ApplicationDbContext _context; // �������� ���� ������ ��� ������ � ��������������

        // �����������, ���������������� �������� ���� ������
        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel(); // ������ �����

        [TempData]
        public string? ErrorMessage { get; set; } // ��� ����������� ��������� �� ������� ����� ���������

        public class InputModel
        {
            [Required(ErrorMessage = "��� ������������ �����������.")]
            [Display(Name = "��� ������������")]
            public string Name { get; set; } = string.Empty; // ��� ������������

            [Required(ErrorMessage = "������ ����������.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty; // ������

            [Display(Name = "��������� ����?")]
            public bool RememberMe { get; set; } // ��������� ������������
        }

        // ����� ��� ��������� GET-������� � ��������
        public IActionResult OnGet()
        {
            // ���� ���� ��������� �� ������, ���������� ���
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            return Page(); // ������ ���������� ��������
        }

        // ����� ��� ��������� POST-������� ��� �������� �����
        public async Task<IActionResult> OnPostAsync()
        {
            // �������� ������������ ��������� ������ ������
            if (!ModelState.IsValid)
            {
                return Page(); // ���� ������ ������������, ���������� �������� � ��������
            }

            // ����� ������������ � ���� ������ �� �����
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == Input.Name); // ���� ������������ ������ �� �����

            // ��������, ������ �� ������������
            if (existingUser == null || existingUser.Password != Input.Password) // ��������� ����� � ������
            {
                // ���������� ������ � ModelState ��� ������ �� ��������
                ModelState.AddModelError(string.Empty, "�������� ��� ������������ ��� ������.");
                return Page(); // ���������� �������� � ���������� �� ������
            }

            // ���� ������������ ������, ��������� ��� � ������
            HttpContext.Session.SetInt32("UserId", existingUser.Id);
            HttpContext.Session.SetString("UserEmail", existingUser.Name);
            HttpContext.Session.SetInt32("UserRoleId", existingUser.RoleId); // ��������� ���� ������������ � ������

            // ��������� ������� "��������� ����" (����� ����� �������� ������ ��� ��������� ����)
            if (Input.RememberMe)
            {
                // ����� ����� �������� ������ ��� ����������� ������������ ����� ����
                // ��������, ���������� ���� � ����-������ �� ��������� ���
            }
            return RedirectToPage("/Index");// ���������� �������� � ���������� �� ������

        }

    }
}