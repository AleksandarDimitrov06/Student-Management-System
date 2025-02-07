using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Models;

namespace Student_Management_System.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public EnrollmentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public IActionResult Index()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Include(e => e.Teacher)
                .ToList();
            return View(enrollments);
        }

        // GET: Enrollments/Details/5
        public IActionResult Details(Guid id)
        {
            var enrollment = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Include(e => e.Teacher)
                .FirstOrDefault(e => e.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Students"] = _context.Students.ToList();
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Teachers"] = _context.Teachers.ToList();
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                enrollment.EnrollmentId = Guid.NewGuid();

              
                enrollment.Student = _context.Students.Find(enrollment.StudentId);
                enrollment.Course = _context.Courses.Find(enrollment.CourseId);
                enrollment.Teacher = _context.Teachers.Find(enrollment.TeacherId);

                if (enrollment.Student == null || enrollment.Course == null || enrollment.Teacher == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Student, Course, or Teacher.");
                    ViewData["Students"] = _context.Students.ToList();
                    ViewData["Courses"] = _context.Courses.ToList();
                    ViewData["Teachers"] = _context.Teachers.ToList();
                    return View(enrollment);
                }
             
                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Students"] = _context.Students.ToList();
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Teachers"] = _context.Teachers.ToList();
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Students"] = _context.Students.ToList();
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Teachers"] = _context.Teachers.ToList();
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(enrollment);
                _context.SaveChanges();
                return RedirectToAction(nameof(Create));
            }
            ViewData["Students"] = _context.Students.ToList();
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Teachers"] = _context.Teachers.ToList();
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

