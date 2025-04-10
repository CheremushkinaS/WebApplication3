using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication3.Model.UserAccount;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    public class CourseSeachController : Controller
    {
        private readonly CourseSearchService _searchService;
        private readonly ApplicationDbContext _context;

        public CourseSeachController(
            CourseSearchService searchService,
            ApplicationDbContext context)
        {
            _searchService = searchService;
            _context = context;
        }

        // Главная страница
        public ActionResult Index(int? page, string search, int? categoryId)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var courses = _searchService.Search(
                query: search,
                categoryId: categoryId,
                pageNumber: pageNumber, // Добавьте этот параметр
                pageSize: pageSize);

            return View(courses);
        }

        // Курсы преподавателя
        public ActionResult TeacherCourses(int? page, string search, int? categoryId)
        {
            // Получаем ID преподавателя из сессии
            var teacherId = HttpContext.Session.GetInt32("UserId");
            if (teacherId == null || HttpContext.Session.GetInt32("UserRoleId") != 2)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var courses = _searchService.Search(
                query: search,
                categoryId: categoryId,
                teacherId: teacherId,
                pageNumber: pageNumber,
                pageSize: pageSize);

            ViewBag.Categories = _context.Categories.ToList();
            return View(courses);
        }

        // Админ-панель
        public ActionResult AdminCourses(int? page, string search, int? categoryId, int? teacherId)
        {
            if (HttpContext.Session.GetInt32("UserRoleId") != 1)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var courses = _searchService.Search(
                query: search,
                categoryId: categoryId,
                teacherId: teacherId,
                pageNumber: pageNumber,
                pageSize: pageSize);

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Teachers = _context.Users
                .Where(u => u.RoleId == 2)
                .ToList();

            return View(courses);
        }
    }
}
