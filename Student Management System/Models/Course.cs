namespace Student_Management_System.Models
{
    public class Course
    {
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; } = null;
        public string? Description { get; set; } = null;


        public ICollection<Enrollment>? Enrollments { get; set; } = null;
    }
}
