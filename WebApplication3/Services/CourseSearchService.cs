using Microsoft.EntityFrameworkCore;
using PagedList;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Services
{
    public class CourseSearchService
    {
        private readonly ApplicationDbContext _context;

        public CourseSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Course>> SearchAsync(
            string query = null,
            int? categoryId = null,
            int? teacherId = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var queryable = _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.CourseCategories)
                .ThenInclude(cc => cc.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                queryable = queryable.Where(c =>
                    c.CourseCategories.Any(cc => cc.category_id == categoryId));
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                queryable = queryable.Where(c =>
                    c.title.Contains(query) ||
                    c.desceiption.Contains(query) ||
                    c.CourseCategories.Any(cc => cc.Category.Name.Contains(query)));
            }

            if (teacherId.HasValue)
            {
                queryable = queryable.Where(c => c.TeacherId == teacherId);
            }

            return await PaginatedList<Course>.CreateAsync(
                queryable.OrderByDescending(c => c.id),
                pageIndex,
                pageSize);
        }
        // Добавьте этот метод
        public IPagedList<Course> Search(
            string query = null,
            int? categoryId = null,
            int? teacherId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var queryable = BuildQuery(query, categoryId, teacherId);
            return queryable.ToPagedList(pageNumber, pageSize);
        }

        private IQueryable<Course> BuildQuery(
            string query,
            int? categoryId,
            int? teacherId)
        {
            var courses = _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.CourseCategories)
                .ThenInclude(cc => cc.Category)
                .AsQueryable();

            // Добавьте фильтры
            if (!string.IsNullOrWhiteSpace(query))
            {
                courses = courses.Where(c =>
                    c.title.Contains(query) ||
                    c.desceiption.Contains(query));
            }

            if (categoryId.HasValue)
            {
                courses = courses.Where(c =>
                    c.CourseCategories.Any(cc => cc.category_id == categoryId));
            }

            if (teacherId.HasValue)
            {
                courses = courses.Where(c => c.TeacherId == teacherId);
            }

            return courses.OrderByDescending(c => c.id);
        }
    }
}
