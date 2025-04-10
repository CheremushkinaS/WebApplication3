using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Pages.Admin.Course
{
    public class CreateCourseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateCourseModel(ApplicationDbContext context)
        {
            _context = context;
            ExistingCategories = new List<SelectListItem>();
        }

        [BindProperty]
        public CourseInputModel CourseInput { get; set; } = new();

        public List<SelectListItem> ExistingCategories { get; set; }

        public class CourseInputModel
        {
            public string title { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public decimal Discount { get; set; }
            public List<int> SelectedCategoryIds { get; set; } = new();
            public string NewCategoriesInput { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            try
            {
                ExistingCategories = await _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.id.ToString(),
                        Text = c.Name
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки категорий: {ex}");
                ExistingCategories = new List<SelectListItem>();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Создание курса
                var course = new Model.UserAccount.Course {

                    title = CourseInput.title,
                    desceiption = CourseInput.Description,
                    price = CourseInput.Price,
                    discount = CourseInput.Discount,
                    TeacherId = 1
                };
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                // Получаем ВСЕ выбранные категории
                var allSelectedCategories = new List<int>();

                // 1. Добавляем явно выбранные существующие категории
                if (CourseInput.SelectedCategoryIds != null)
                {
                    allSelectedCategories.AddRange(CourseInput.SelectedCategoryIds);
                }

                // 2. Добавляем новые категории
                var newCategoryNames = CourseInput.NewCategoriesInput?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim())
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .ToList() ?? new List<string>();

                foreach (var categoryName in newCategoryNames)
                {
                    var existingCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name == categoryName);

                    if (existingCategory == null)
                    {
                        var newCategory = new WebApplication3.Model.UserAccount.Category { Name = categoryName };
                        _context.Categories.Add(newCategory);
                        await _context.SaveChangesAsync();
                        allSelectedCategories.Add(newCategory.id);
                    }
                    else
                    {
                        allSelectedCategories.Add(existingCategory.id);
                    }
                }

                // 3. Сохраняем ВСЕ связи
                foreach (var categoryId in allSelectedCategories.Distinct())
                {
                    _context.Course_Categories.Add(new Course_Categories
                    {
                        course_id = course.id,
                        category_id = categoryId
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToPage("/Admin/Course/Index");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                await LoadCategories();
                return Page();
            }
        }

      
    }
}