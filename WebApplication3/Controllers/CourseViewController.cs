// Controllers/CoursesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Courses/Detailed/5
        public async Task<IActionResult> Detailed(int id)
        {
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null) return RedirectToPage("/Account/Login");

            var course = await _context.Courses
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Materials)
                .FirstOrDefaultAsync(c => c.id == id && c.TeacherId == teacherId);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
    }
}