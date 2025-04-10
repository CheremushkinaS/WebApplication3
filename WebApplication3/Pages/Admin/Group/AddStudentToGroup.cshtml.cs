using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin
{
    public class AddStudentToGroupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddStudentToGroupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int GroupId { get; set; }

        [BindProperty]
        public int SelectedStudentId { get; set; }

        public List<SelectListItem> Students { get; set; }
        public List<StudentInfo> CurrentStudents { get; set; }

        public class StudentInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public async Task OnGetAsync(int groupId)
        {
            GroupId = groupId;

            await LoadStudents();
            await LoadCurrentStudents();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadStudents();
                await LoadCurrentStudents();
                return Page();
            }

            try
            {
                var existingLink = await _context.Group_Student
                    .FirstOrDefaultAsync(gs => gs.id_Group == GroupId && gs.id_student == SelectedStudentId);

                if (existingLink == null)
                {
                    _context.Group_Student.Add(new Group_Student
                    {
                        id_Group = GroupId,
                        id_student = SelectedStudentId
                    });
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("./AddStudentToGroup", new { groupId = GroupId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                await LoadStudents();
                await LoadCurrentStudents();
                return Page();
            }
        }

        private async Task LoadStudents()
        {
            Students = await _context.Users
                .Where(u => u.RoleId == 3) // Фильтр по роли "Студент"
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.Name} ({u.Email})"
                })
                .ToListAsync();
        }

        private async Task LoadCurrentStudents()
        {
            CurrentStudents = await _context.Group_Student
                .Where(gs => gs.id_Group == GroupId)
                .Include(gs => gs.Student)
                .Select(gs => new StudentInfo
                {
                    Id = gs.id_student,
                    Name = gs.Student.Name,
                    Email = gs.Student.Email
                })
                .ToListAsync();
        }
    }
}