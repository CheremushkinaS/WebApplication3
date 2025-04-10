using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin
{
    public class GroupListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GroupListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<WebApplication3.Model.UserAccount.Group> Group { get; set; }


        public async Task OnGetAsync()
        {
            Group = await _context.Groups.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
