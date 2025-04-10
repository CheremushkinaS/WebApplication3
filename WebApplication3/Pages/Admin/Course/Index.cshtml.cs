using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;
using WebApplication3.Services;

namespace WebApplication3.Pages.Admin.Course
{
    public class IndexModel : PageModel
    {
        private readonly CourseSearchService _searchService;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            CourseSearchService searchService,
            ApplicationDbContext context)
        {
            _searchService = searchService;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? TeacherId { get; set; }

        public PaginatedList<WebApplication3.Model.UserAccount.Course> Courses { get; set; }
        public List<WebApplication3.Model.UserAccount.Category> Categories { get; set; }
        public List<WebApplication3.Model.UserAccount.User> Teachers { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            try
            {
                int pageSize = 10;
                int currentPage = pageIndex ?? 1;

                // Инициализация фильтров
                Categories = await _context.Categories.ToListAsync();
                Teachers = await _context.Users
                    .Where(u => u.RoleId == 2)
                    .ToListAsync();

                // Базовый запрос с включением связанных данных
                IQueryable<WebApplication3.Model.UserAccount.Course> query = _context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.CourseCategories)
                    .ThenInclude(cc => cc.Category)
                    .OrderByDescending(c => c.id);

                // Применение фильтров
                if (!string.IsNullOrEmpty(SearchString))
                {
                    query = query.Where(c =>
                        c.title.Contains(SearchString) ||
                        c.desceiption.Contains(SearchString) ||
                        c.CourseCategories.Any(cc => cc.Category.Name.Contains(SearchString)));
                }

                if (CategoryId.HasValue)
                {
                    query = query.Where(c =>
                        c.CourseCategories.Any(cc => cc.category_id == CategoryId));
                }

                if (TeacherId.HasValue)
                {
                    query = query.Where(c => c.TeacherId == TeacherId);
                }

                // Применение пагинации
                Courses = await PaginatedList<WebApplication3.Model.UserAccount.Course>.CreateAsync(
                    query.AsNoTracking(),
                    currentPage,
                    pageSize);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка при загрузке курсов: {ex}");
                Courses = new PaginatedList<WebApplication3.Model.UserAccount.Course>(new List<WebApplication3.Model.UserAccount.Course>(), 0, 1, 10);
            }
        }
    }
}