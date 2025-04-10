using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model.UserAccount;

namespace WebApplication3.Pages.Admin.Course
{
    public class EditModel : PageModel
    {
        private readonly WebApplication3.Model.UserAccount.ApplicationDbContext _context;

        public EditModel(WebApplication3.Model.UserAccount.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Model.UserAccount.Course Courses { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses =  await _context.Courses.FirstOrDefaultAsync(m => m.id == id);
            if (courses == null)
            {
                return NotFound();
            }
            Courses = courses;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Courses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursesExists(Courses.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CoursesExists(int id)
        {
            return _context.Courses.Any(e => e.id == id);
        }
    }
}
