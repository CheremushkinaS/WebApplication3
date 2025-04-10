using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public IList<WebApplication3.Model.UserAccount.User> Users { get; set; } = new List<WebApplication3.Model.UserAccount.User>();

        public async Task OnGetAsync()
        {
            var query = _context.Users
                .Include(u => u.Role)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(u =>
                    u.Name.Contains(SearchString) ||
                    u.Email.Contains(SearchString) ||
                    u.Role.name.Contains(SearchString));
            }

            Users = await query
                .OrderBy(u => u.Name)
                .ToListAsync();
        }
    }
}