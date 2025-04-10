// CreateLessonModel.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApplication3.Model.UserAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Antiforgery;

namespace WebApplication3.Pages.Teacher
{
    public class CreateLessonModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly long _fileSizeLimit = 1073741824; // 1 GB

        public IConfiguration Configuration => _configuration;

        [BindProperty(SupportsGet = true)]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [BindProperty]
        public LessonInputModel Input { get; set; } = new();

        public List<Material> ExistingMaterials { get; set; } = new();

        // Добавляем свойства для уроков курса
        public List<Lesson> CourseLessons { get; set; } = new();
        public int? CurrentLessonId { get; set; }
        public CreateLessonModel(
            ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }

        public class LessonInputModel
        {
            [Required(ErrorMessage = "Название урока обязательно")]
            [MaxLength(200, ErrorMessage = "Максимальная длина названия - 200 символов")]
            [Display(Name = "Название урока")]
            public string Title { get; set; } = string.Empty;

            public List<MaterialInputModel> NewMaterials { get; set; } = new();
        }

        public class MaterialInputModel
        {
            [Required(ErrorMessage = "Введите название материала")]
            [Display(Name = "Название материала")]
            public string Title { get; set; } = string.Empty;

            [Display(Name = "Содержимое материала")]
            public string Content { get; set; } = string.Empty;
        }
    
        public static string TruncateContent(string content, int maxLength)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            return content.Length <= maxLength
                ? content
                : content.Substring(0, maxLength) + "...";
        }
        public async Task<IActionResult> OnGetAsync(int id )
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
            // Инициализация первого материала
            if (Input.NewMaterials.Count == 0)
            {
                Input.NewMaterials.Add(new MaterialInputModel());
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> OnPostUploadImageAsync()
        {
            try
            {
                var file = Request.Form.Files["file"];
                if (file == null || file.Length == 0)
                    return new JsonResult(new { error = "Файл не выбран" });

                var isImage = file.ContentType.StartsWith("image/");
                var isVideo = file.ContentType.StartsWith("video/");

                if (!isImage && !isVideo)
                    return new JsonResult(new { error = "Недопустимый тип файла" });

                if (file.Length > _fileSizeLimit)
                    return new JsonResult(new { error = $"Максимальный размер файла: {_fileSizeLimit / 1024 / 1024} MB" });

                var subfolder = isImage ? "images" : "videos";
                var fileUrl = await ProcessUploadedFile(file, subfolder);

                var htmlContent = isImage
                    ? $"<img src='{fileUrl}' alt='Uploaded image' class='uploaded-media'>"
                    : $"<div class='video-container'><video controls class='uploaded-video'><source src='{fileUrl}' type='{file.ContentType}'></video></div>";

                return new JsonResult(new { location = fileUrl, html = htmlContent });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        private async Task<string> ProcessUploadedFile(IFormFile file, string subfolder)
        {
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);
            var fileUrl = $"/uploads/{subfolder}/{fileName}";

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileUrl;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                ExistingMaterials = await _context.Materials
                    .Include(m => m.Lesson)
                    .Where(m => m.Lesson.CourseId == CourseId)
                    .ToListAsync();
                return Page();
            }

            try
            {
                var lesson = new Lesson
                {
                    Title = Input.Title,
                    CourseId = id,
                    CreatedAt = DateTime.UtcNow,
                    Materials = new List<Material>()
                };

                foreach (var materialInput in Input.NewMaterials)
                {
                    var material = new Material
                    {
                        Title = materialInput.Title,
                        Content = materialInput.Content,
                        CreatedAt = DateTime.UtcNow
                    };
                    lesson.Materials.Add(material);
                }

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();

                return RedirectToPage("./EditCourse", new {id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка создания урока: {ex.Message}");
                return Page();
            }
        }
    }
}