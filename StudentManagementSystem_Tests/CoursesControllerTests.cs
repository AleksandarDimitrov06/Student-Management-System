using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Controllers;
using Student_Management_System.Data;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StudentManagementSystem_Tests
{
    public class CoursesControllerTests
    {
        private readonly ApplicationDBContext _context;
        private readonly CoursesController _controller;
        private readonly List<Course> _courses;

        public CoursesControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new ApplicationDBContext(options);
            _controller = new CoursesController(_context);

            _courses = new List<Course>
    {
        new Course { CourseId = Guid.NewGuid(), CourseName = "Course 1", Description = "Description 1" },
        new Course { CourseId = Guid.NewGuid(), CourseName = "Course 2", Description = "Description 2" }
    };

            _context.Courses.AddRange(_courses);
            _context.SaveChanges();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfCourses()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Course>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Details_ReturnsViewResult_WithCourse()
        {
            // Arrange
            var courseId = _courses[0].CourseId;

            // Act
            var result = _controller.Details(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Course>(viewResult.ViewData.Model);
            Assert.Equal(courseId, model.CourseId);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenCourseNotFound()
        {
            // Act
            var result = _controller.Details(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var course = new Course { CourseName = "New Course", Description = "New Description" };

            // Act
            var result = _controller.Create(course);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("CourseName", "Required");
            var course = new Course { Description = "New Description" };

            // Act
            var result = _controller.Create(course);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Course>(viewResult.ViewData.Model);
            Assert.Equal(course, model);
        }

        [Fact]
        public void Edit_Get_ReturnsViewResult_WithCourse()
        {
            // Arrange
            var courseId = _courses[0].CourseId;

            // Act
            var result = _controller.Edit(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Course>(viewResult.ViewData.Model);
            Assert.Equal(courseId, model.CourseId);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenCourseNotFound()
        {
            // Act
            var result = _controller.Edit(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var course = _courses[0];

            // Act
            var result = _controller.Edit(course.CourseId, course);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_Post_ReturnsBadRequest_WhenCourseIdDoesNotMatch()
        {
            // Arrange
            var course = _courses[0];

            // Act
            var result = _controller.Edit(Guid.NewGuid(), course);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("CourseName", "Required");
            var course = _courses[0];

            // Act
            var result = _controller.Edit(course.CourseId, course);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Course>(viewResult.ViewData.Model);
            Assert.Equal(course, model);
        }

        [Fact]
        public void Delete_Post_ReturnsRedirectToActionResult_WhenCourseIsDeleted()
        {
            // Arrange
            var courseId = _courses[0].CourseId;

            // Act
            var result = _controller.Delete(courseId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_Post_ReturnsNotFound_WhenCourseNotFound()
        {
            // Act
            var result = _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}