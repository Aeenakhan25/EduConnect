namespace AcademicWebPortal.Models;

// [SRP] Single Responsibility Principle:
//   Enrollment's only responsibility is to represent the relationship between a
//   Student and a Course for a given semester. All business rules around enrolling
//   or dropping are kept in ICourseService / CourseService, not here.
public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string Semester { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; } = true;

    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
