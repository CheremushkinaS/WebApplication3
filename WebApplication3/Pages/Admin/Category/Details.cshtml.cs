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
    public class DetailsModel : PageModel
    {
        private readonly WebApplication3.Model.UserAccount.ApplicationDbContext _context;

        public DetailsModel(WebApplication3.Model.UserAccount.ApplicationDbContext context)
        {
            _context = context;
        }

        public WebApplication3.Model.UserAccount.Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }
    }
}
