using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Models;

namespace Student_Management_System.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDBContext _context;
        public CoursesController(ApplicationDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }

        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            var course = _context.Courses
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Teacher)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefault(e => e.CourseId == id);






            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

          [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
         [Authorize(Roles = "Admin")]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseId = Guid.NewGuid();
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
