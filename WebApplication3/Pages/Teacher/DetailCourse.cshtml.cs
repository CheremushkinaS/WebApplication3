using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Teacher
{
    public class DetailCourseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailCourseModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null) return RedirectToPage("/Account/Login");

            Course = await _context.Courses
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Materials)
                .FirstOrDefaultAsync(c => c.id == id && c.TeacherId == teacherId);

            if (Course == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}