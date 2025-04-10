using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc; // Необходимые пространства имен для работы с MVC
using Microsoft.AspNetCore.Mvc.RazorPages; // Подключение для работы с Razor Pages
using Microsoft.EntityFrameworkCore; // Подключение для работы с EF Core
using System; // Основные типы и функции
using System.Collections.Generic; // Импортирование пространств для IList
using System.ComponentModel.DataAnnotations; // Импортирование атрибутов данных
using System.Threading.Tasks; // Подключение для асинхронных операций
using WebApplication3.Model.UserAccount; // Подключение модели пользовательской учетной записи

namespace WebApplication3.Pages.Account // Пространство имен для страниц аккаунта
{
    public class LoginModel : PageModel // Модель страницы для входа
    {
        private readonly ApplicationDbContext _context; // Контекст базы данных для работы с пользователями

        // Конструктор, инициализирующий контекст базы данных
        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel(); // Модель ввода

        [TempData]
        public string? ErrorMessage { get; set; } // Для отображения сообщений об ошибках между запросами

        public class InputModel
        {
            [Required(ErrorMessage = "Имя пользователя обязательно.")]
            [Display(Name = "Имя пользователя")]
            public string Name { get; set; } = string.Empty; // Имя пользователя

            [Required(ErrorMessage = "Пароль обязателен.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty; // Пароль

            [Display(Name = "Запомнить меня?")]
            public bool RememberMe { get; set; } // Запомнить пользователя
        }

        // Метод для обработки GET-запроса к странице
        public IActionResult OnGet()
        {
            // Если есть сообщение об ошибке, показываем его
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            return Page(); // Просто возвращаем страницу
        }

        // Метод для обработки POST-запроса при отправке формы
        public async Task<IActionResult> OnPostAsync()
        {
            // Проверка корректности введенных данных модели
            if (!ModelState.IsValid)
            {
                return Page(); // Если данные некорректные, возвращаем страницу с ошибками
            }

            // Поиск пользователя в базе данных по имени
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == Input.Name); // Ищем пользователя только по имени

            // Проверка, найден ли пользователь
            if (existingUser == null || existingUser.Password != Input.Password) // Проверяем также и пароль
            {
                // Добавление ошибки в ModelState для вывода на страницу
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
                return Page(); // Возвращаем страницу с сообщением об ошибке
            }

            // Если пользователь найден, сохраняем его в сессии
            HttpContext.Session.SetInt32("UserId", existingUser.Id);
            HttpContext.Session.SetString("UserEmail", existingUser.Name);
            HttpContext.Session.SetInt32("UserRoleId", existingUser.RoleId); // Сохраняем роль пользователя в сессии

            // Реализуем функцию "Запомнить меня" (здесь можно добавить логику для настройки куки)
            if (Input.RememberMe)
            {
                // Здесь можно добавить логику для запоминания пользователя через куки
                // Например, установить куку с авто-входом на следующий раз
            }
            return RedirectToPage("/Index");// Возвращаем страницу с сообщением об ошибке

        }

    }
}