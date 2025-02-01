namespace Student_Management_System.Models
{
    public class Teacher
    {
        public Guid TeacherId { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Specialization { get; set; } = null;


        public ICollection<Enrollment>? Enrollments { get; set; } = null;
    }
}
