using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin.Group
{
    public class GroupCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GroupCreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> Courses { get; set; }

        [BindProperty]
        public WebApplication3.Model.UserAccount.Group Group { get; set; }

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.title
                })
                .ToListAsync();
          
          
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
           

            _context.Groups.Add(Group); // Исправлено на Groups!
            await _context.SaveChangesAsync();

            return RedirectToPage("GroupListModel");
        }
    }
}
