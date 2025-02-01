namespace Student_Management_System.Models
{
    public class Enrollment
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid CourseId { get; set; }

        public Student? Student { get; set; } = null;
        public Course? Course { get; set; } = null;
        public Teacher? Teacher { get; set; } = null;

        public DateTime? EnrollmentDate { get; set; } = null;
    }
}
