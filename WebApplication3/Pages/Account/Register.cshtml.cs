using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WebApplication3.Model.UserAccount;
using static WebApplication3.Service.UserService;
using WebApplication3.Service;

namespace WebApplication3.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public RegisterModel(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var registrationDto = new UserRegistrationDto
            {
                Name = Input.Name,
                Email = Input.Email,
                Password = Input.Password
            };

            var result = await _userService.RegisterUserAsync(registrationDto);

            if (!result.UseSuccess)
            {
                ModelState.AddModelError(string.Empty, result.UseError);
                return Page();
            }

            // Установка сессии
            HttpContext.Session.SetString("UserEmail", result.User.Email);
            HttpContext.Session.SetInt32("UserRoleId", result.User.RoleId);

            return RedirectToPage("/Index");
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Имя пользователя обязательно.")]
            [Display(Name = "Имя пользователя")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email обязателен.")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Пароль обязателен.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }
    }
}

