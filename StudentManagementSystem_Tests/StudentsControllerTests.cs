using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Controllers;
using Student_Management_System.Data;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class StudentsControllerTests
{
    private readonly ApplicationDBContext _context;
    private readonly StudentsController _controller;
    private readonly List<Student> _students;

    public StudentsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _context = new ApplicationDBContext(options);
        _controller = new StudentsController(_context);

        _students = new List<Student>
        {
            new Student { StudentId = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new Student { StudentId = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        _context.Students.AddRange(_students);
        _context.SaveChanges();
    }

    [Fact]
    public void Index_ReturnsViewResult_WithListOfStudents()
    {
        var result = _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Student>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public void Details_ReturnsViewResult_WithStudent()
    {
        var studentId = _students[0].StudentId;
        var result = _controller.Details(studentId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Student>(viewResult.ViewData.Model);
        Assert.Equal(studentId, model.StudentId);
    }

    [Fact]
    public void Details_ReturnsNotFound_WhenStudentNotFound()
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
        var student = new Student { FirstName = "New", LastName = "Student" };
        var result = _controller.Create(student);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("FirstName", "Required");
        var student = new Student { LastName = "Invalid" };
        var result = _controller.Create(student);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Student>(viewResult.ViewData.Model);
        Assert.Equal(student, model);
    }

    [Fact]
    public void Edit_Get_ReturnsViewResult_WithStudent()
    {
        var studentId = _students[0].StudentId;
        var result = _controller.Edit(studentId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Student>(viewResult.ViewData.Model);
        Assert.Equal(studentId, model.StudentId);
    }

    [Fact]
    public void Edit_Get_ReturnsNotFound_WhenStudentNotFound()
    {
        var result = _controller.Edit(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
    {
        var student = _students[0];
        var result = _controller.Edit(student.StudentId, student);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void Edit_Post_ReturnsBadRequest_WhenStudentIdDoesNotMatch()
    {
        var student = _students[0];
        var result = _controller.Edit(Guid.NewGuid(), student);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("FirstName", "Required");
        var student = _students[0];
        var result = _controller.Edit(student.StudentId, student);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Student>(viewResult.ViewData.Model);
        Assert.Equal(student, model);
    }

    [Fact]
    public void Delete_Post_ReturnsRedirectToActionResult_WhenStudentIsDeleted()
    {
        var studentId = _students[0].StudentId;
        var result = _controller.Delete(studentId);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void Delete_Post_ReturnsNotFound_WhenStudentNotFound()
    {
        var result = _controller.Delete(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(result);
    }
}
