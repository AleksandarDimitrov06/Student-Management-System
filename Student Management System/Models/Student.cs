using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Student
    {
        public Guid StudentId { get; set; }

        public string ?FirstName { get; set; } = null;


        public string? LastName { get; set; } = null;


        public ICollection<Enrollment>? Enrollments { get; set; } = null;
    }
}
