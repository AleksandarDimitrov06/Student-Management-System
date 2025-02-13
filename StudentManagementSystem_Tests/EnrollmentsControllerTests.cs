using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Controllers;
using Student_Management_System.Data;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class EnrollmentsControllerTests
{
    private readonly ApplicationDBContext _context;
    private readonly EnrollmentsController _controller;
    private readonly List<Student> _students;
    private readonly List<Course> _courses;
    private readonly List<Teacher> _teachers;
    private readonly List<Enrollment> _enrollments;

    public EnrollmentsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _context = new ApplicationDBContext(options);
        _controller = new EnrollmentsController(_context);

        _students = new List<Student>
        {
            new Student { StudentId = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new Student { StudentId = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        _courses = new List<Course>
        {
            new Course { CourseId = Guid.NewGuid(), CourseName = "Math", Description = "Basic Math" },
            new Course { CourseId = Guid.NewGuid(), CourseName = "Science", Description = "Basic Science" }
        };

        _teachers = new List<Teacher>
        {
            new Teacher { TeacherId = Guid.NewGuid(), FirstName = "Alice", LastName = "Brown" },
            new Teacher { TeacherId = Guid.NewGuid(), FirstName = "Bob", LastName = "White" }
        };

        _enrollments = new List<Enrollment>
        {
            new Enrollment { EnrollmentId = Guid.NewGuid(), Student = _students[0], Course = _courses[0], Teacher = _teachers[0] },
            new Enrollment { EnrollmentId = Guid.NewGuid(), Student = _students[1], Course = _courses[1], Teacher = _teachers[1] }
        };

        _context.Students.AddRange(_students);
        _context.Courses.AddRange(_courses);
        _context.Teachers.AddRange(_teachers);
        _context.Enrollments.AddRange(_enrollments);
        _context.SaveChanges();
    }

    [Fact]
    public void Index_ReturnsViewResult_WithListOfEnrollments()
    {
        var result = _controller.Index();
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Enrollment>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public void Details_ReturnsViewResult_WithEnrollment()
    {
        var enrollmentId = _enrollments[0].EnrollmentId;
        var result = _controller.Details(enrollmentId);
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Enrollment>(viewResult.ViewData.Model);
        Assert.Equal(enrollmentId, model.EnrollmentId);
    }

    [Fact]
    public void Details_ReturnsNotFound_WhenEnrollmentNotFound()
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
        var enrollment = new Enrollment { StudentId = _students[0].StudentId, CourseId = _courses[0].CourseId, TeacherId = _teachers[0].TeacherId };
        var result = _controller.Create(enrollment);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("StudentId", "Required");
        var enrollment = new Enrollment();
        var result = _controller.Create(enrollment);
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Enrollment>(viewResult.ViewData.Model);
        Assert.Equal(enrollment, model);
    }

    [Fact]
    public void Edit_Get_ReturnsViewResult_WithEnrollment()
    {
        var enrollmentId = _enrollments[0].EnrollmentId;
        var result = _controller.Edit(enrollmentId);
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Enrollment>(viewResult.ViewData.Model);
        Assert.Equal(enrollmentId, model.EnrollmentId);
    }

    [Fact]
    public void Edit_Get_ReturnsNotFound_WhenEnrollmentNotFound()
    {
        var result = _controller.Edit(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_Post_ReturnsRedirectToActionResult_WhenEnrollmentIsDeleted()
    {
        var enrollmentId = _enrollments[0].EnrollmentId;
        var result = _controller.DeleteConfirmed(enrollmentId);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void Delete_Post_ReturnsNotFound_WhenEnrollmentNotFound()
    {
        var result = _controller.DeleteConfirmed(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(result);
    }
}
