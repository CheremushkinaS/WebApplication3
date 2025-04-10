using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Teacher
{
    public class TeacherCoursesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TeacherCoursesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get; set; } = new List<Course>();

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null) return RedirectToPage("/Account/Login");

            Courses = await _context.Courses
                .Include(c => c.Groups) // Добавляем загрузку групп
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Materials) // Загрузка материалов уроков
                .Where(c => c.TeacherId == teacherId)
                .AsNoTracking() // Для оптимизации
                .ToListAsync();

            return Page();
        }
    }
}