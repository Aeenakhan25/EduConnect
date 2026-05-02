using AcademicWebPortal.Models;
namespace AcademicWebPortal.Interfaces;

// [ISP] Interface Segregation Principle:
//   ICourseService groups ONLY course-related operations (CRUD + enrollment actions).
//   It is deliberately separate from IStudentService and IGradeService so that
//   consumers only depend on the course behaviour they actually need.
//
// [DIP] Dependency Inversion Principle:
//   Pages and services that manage courses depend on this interface, not on the
//   concrete CourseService. A different implementation (e.g. database-backed) can
//   be swapped in Program.cs without touching any consumer.
public interface ICourseService
{
    Task<List<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdAsync(int id);
    Task AddCourseAsync(Course course);
    Task UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(int id);
    Task EnrollStudentAsync(int studentId, int courseId, string semester);
    Task DropCourseAsync(int studentId, int courseId, string semester);
    Task<bool> IsStudentEnrolledAsync(int studentId, int courseId, string semester);
    Task<List<Student>> GetEnrolledStudentsAsync(int courseId);
}
