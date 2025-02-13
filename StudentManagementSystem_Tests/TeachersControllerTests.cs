using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Student_Management_System.Controllers;

namespace Student_Management_System.Tests
{
    public class TeachersControllerTests
    {
        private readonly ApplicationDBContext _context;
        private readonly TeachersController _controller;
        private readonly List<Teacher> _teachers;

        public TeachersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDBContext(options);
            _controller = new TeachersController(_context);

            _teachers = new List<Teacher>
            {
                new Teacher { TeacherId = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Specialization = "Math" },
                new Teacher { TeacherId = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Specialization = "Physics" }
            };

            _context.Teachers.AddRange(_teachers);
            _context.SaveChanges();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfTeachers()
        {
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Teacher>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Details_ReturnsViewResult_WithTeacher()
        {
            var teacherId = _teachers[0].TeacherId;
            var result = _controller.Details(teacherId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Teacher>(viewResult.ViewData.Model);
            Assert.Equal(teacherId, model.TeacherId);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenTeacherNotFound()
        {
            var result = _controller.Details(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            var teacher = new Teacher { FirstName = "New", LastName = "Teacher", Specialization = "Biology" };
            var result = _controller.Create(teacher);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var teacher = new Teacher { LastName = "Teacher", Specialization = "Chemistry" };
            var result = _controller.Create(teacher);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Teacher>(viewResult.ViewData.Model);
            Assert.Equal(teacher, model);
        }

        [Fact]
        public void Edit_Get_ReturnsViewResult_WithTeacher()
        {
            var teacherId = _teachers[0].TeacherId;
            var result = _controller.Edit(teacherId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Teacher>(viewResult.ViewData.Model);
            Assert.Equal(teacherId, model.TeacherId);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenTeacherNotFound()
        {
            var result = _controller.Edit(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            var teacher = _teachers[0];
            var result = _controller.Edit(teacher.TeacherId, teacher);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_Post_ReturnsBadRequest_WhenTeacherIdDoesNotMatch()
        {
            var teacher = _teachers[0];
            var result = _controller.Edit(Guid.NewGuid(), teacher);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var teacher = _teachers[0];
            var result = _controller.Edit(teacher.TeacherId, teacher);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Teacher>(viewResult.ViewData.Model);
            Assert.Equal(teacher, model);
        }

        [Fact]
        public void Delete_Post_ReturnsRedirectToActionResult_WhenTeacherIsDeleted()
        {
            var teacherId = _teachers[0].TeacherId;
            var result = _controller.Delete(teacherId);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_Post_ReturnsNotFound_WhenTeacherNotFound()
        {
            var result = _controller.Delete(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }
    }
}