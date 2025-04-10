using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin.Category
{
    public class IndexModel : PageModel
    {
        private readonly WebApplication3.Model.UserAccount.ApplicationDbContext _context;

        public IndexModel(WebApplication3.Model.UserAccount.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<WebApplication3.Model.UserAccount.Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}
