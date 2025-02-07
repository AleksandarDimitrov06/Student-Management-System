using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Models;

namespace Student_Management_System.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDBContext _context;
        public StudentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }

        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            var student = _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course) 
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Teacher) 
                .FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                student.StudentId = Guid.NewGuid();
                student.Enrollments = new List<Enrollment>();
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
       [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [HttpPost]
       [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
