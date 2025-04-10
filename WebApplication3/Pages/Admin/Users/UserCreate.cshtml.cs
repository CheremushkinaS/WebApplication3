using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WebApplication3.Model.UserAccount.User User { get; set; } = new WebApplication3.Model.UserAccount.User ();

        public SelectList Roles { get; set; }
        public SelectList Teachers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadRolesAsync();
            await LoadTeachersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadRolesAsync();
                await LoadTeachersAsync();
                return Page();
            }

            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task LoadRolesAsync()
        {
            var roles = await _context.UserRoles
                .OrderBy(r => r.name)
                .ToListAsync();

            Roles = new SelectList(roles,
                nameof(Role.id),
                nameof(Role.name));
        }

        private async Task LoadTeachersAsync()
        {
            var teachers = await _context.Users
                .Where(u => u.RoleId == 2) // ID роли преподавателя
                .OrderBy(u => u.Name)
                .ToListAsync();

            Teachers = new SelectList(teachers,
                nameof(User.Id),
                nameof(User.Name));
        }
    }
}