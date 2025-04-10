using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Pages.Home
{
    public class CourseDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CourseDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }
        public int? CourseDuration { get; set; }
        public DateTime? NearestGroupStartDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _context.Courses
                .Include(c => c.CourseCategories)
                .ThenInclude(cc => cc.Category)
                .Include(c => c.Teacher)
                .Include(c => c.Groups) // Добавляем связь с группами
                .FirstOrDefaultAsync(c => c.id == id);

            if (Course == null)
            {
                return NotFound();
            }

            // Получаем последнюю актуальную группу
            var latestGroup = Course.Groups
                .Where(g => g.end_date >= DateTime.Today)
                .OrderByDescending(g => g.start_date)
                .FirstOrDefault();

            if (latestGroup != null)
            {
                // Рассчитываем длительность в неделях
                var duration = (latestGroup.end_date - latestGroup.start_date).TotalDays / 7;
                CourseDuration = (int)Math.Ceiling(duration);
                NearestGroupStartDate = latestGroup.start_date;
            }

            return Page();
        }
    }
}