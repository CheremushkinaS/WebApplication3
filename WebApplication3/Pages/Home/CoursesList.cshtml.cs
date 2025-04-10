using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Home
{
    public class CoursesListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CoursesListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Course> Courses { get; set; } = new();

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.CourseCategories)
                .ThenInclude(cc => cc.Category)
                .Include(c => c.Teacher) // Добавлено включение преподавателя
                .ToListAsync();
        }
    }
}