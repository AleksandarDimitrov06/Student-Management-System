using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Management_System.Data;
using Student_Management_System.Models;

namespace Student_Management_System.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ApplicationDBContext _context;
        public TeachersController(ApplicationDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var teachers = _context.Teachers.ToList();
            return View(teachers);
        }

        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            var teacher = _context.Teachers.FirstOrDefault(e => e.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        //  [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public IActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.TeacherId = Guid.NewGuid();
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }



        [HttpPost]
        public IActionResult Edit(Guid id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }
            _context.Teachers.Remove(teacher);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
