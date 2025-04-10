using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Course> FeaturedCourses { get; set; }

        public void OnGet()
        {
            // �������� 3 ��������� ����� ��� �������
            FeaturedCourses = _context.Courses
                .Include(c => c.CourseCategories)
                .ThenInclude(cc => cc.Category)
                .OrderByDescending(c => c.id)
                .Take(3)
                .ToList();
        }
    }
}