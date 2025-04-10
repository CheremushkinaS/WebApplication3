using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly WebApplication3.Model.UserAccount.ApplicationDbContext _context;

        public DetailsModel(WebApplication3.Model.UserAccount.ApplicationDbContext context)
        {
            _context = context;
        }

        public WebApplication3.Model.UserAccount.User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            // Загружаем пользователя с включенной ролью
            User = await _context.Users
                .Include(u => u.RoleId)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (User == null) return NotFound();

            return Page();
        }
    }
}
