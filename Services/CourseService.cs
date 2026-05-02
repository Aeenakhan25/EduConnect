using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Exceptions;
using AcademicWebPortal.Models.Enums;
using AcademicWebPortal.Interfaces;
namespace AcademicWebPortal.Services;

// [SRP] Single Responsibility Principle:
//   Handles all business rules and operations specifically related to Courses
//   (CRUD and enrollments). It does not manage students or grades directly.
public class CourseService : ICourseService
{
    private readonly List<Course> _courses = new()
    {
        new Course { Id = 1, CourseCode = "CS101", Title = "Introduction to Programming", MaxCapacity = 30, Credits = 3, CurrentEnrollmentCount = 0 },
        new Course { Id = 2, CourseCode = "MATH201", Title = "Calculus II", MaxCapacity = 2, Credits = 4, CurrentEnrollmentCount = 0 },
        new Course { Id = 3, CourseCode = "DB301", Title = "Database Systems", MaxCapacity = 25, Credits = 3, CurrentEnrollmentCount = 0 }
    };
    private readonly List<Enrollment> _enrollments = new();
    private readonly List<Enrollment> _dropHistory = new();
    private readonly IStudentService _studentService;

    // [DIP] Dependency Inversion Principle:
    //   CourseService depends on the IStudentService abstraction to fetch
    //   student data, avoiding tight coupling to the concrete StudentService.
    public CourseService(IStudentService studentService)
    {
        _studentService = studentService;
    }
    public Task<List<Course>> GetAllCoursesAsync() => Task.FromResult(_courses);
    public Task<Course?> GetCourseByIdAsync(int id) => Task.FromResult(_courses.FirstOrDefault(c => c.Id == id));

    public Task AddCourseAsync(Course course)
    {
        course.Id = _courses.Any() ? _courses.Max(c => c.Id) + 1 : 1;
        _courses.Add(course);
        return Task.CompletedTask;
    }
    public Task UpdateCourseAsync(Course course)
    {
        var existing = _courses.FirstOrDefault(c => c.Id == course.Id);
        if (existing != null)
        {
            existing.Title = course.Title;
            existing.CourseCode = course.CourseCode;
            existing.Credits = course.Credits;
            existing.MaxCapacity = course.MaxCapacity;
            existing.Description = course.Description;
        }
        return Task.CompletedTask;
    }
    public Task DeleteCourseAsync(int id)
    {
        _courses.RemoveAll(c => c.Id == id);
        return Task.CompletedTask;
    }
    public Task<bool> IsStudentEnrolledAsync(int studentId, int courseId, string semester)
    {
        return Task.FromResult(_enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId && e.Semester == semester && e.IsActive));
    }
    public async Task<List<Student>> GetEnrolledStudentsAsync(int courseId)
    {
        var studentIds = _enrollments
            .Where(e => e.CourseId == courseId && e.IsActive)
            .Select(e => e.StudentId)
            .ToList();

        var allStudents = await _studentService.GetAllStudentsAsync();
        return allStudents.Where(s => studentIds.Contains(s.Id)).ToList();
    }
    public Task EnrollStudentAsync(int studentId, int courseId, string semester)
    {
        var course = _courses.FirstOrDefault(c => c.Id == courseId);
        if (course == null) return Task.CompletedTask;

        if (course.CurrentEnrollmentCount >= course.MaxCapacity)
            throw new CourseFullException();
        var hasDroppedBefore = _dropHistory.Any(e => e.StudentId == studentId && e.CourseId == courseId && e.Semester == semester);
        if (hasDroppedBefore)
            throw new InvalidOperationException("Strict Business Rule: Cannot re-enroll in a course you dropped during the same semester.");

        if (_enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId && e.Semester == semester && e.IsActive))
            throw new InvalidOperationException("Already enrolled.");

        var enrollment = new Enrollment
        {
            Id = _enrollments.Count + 1,
            StudentId = studentId,
            CourseId = courseId,
            Semester = semester,
            EnrollmentDate = DateTime.Now,
            IsActive = true
        };

        _enrollments.Add(enrollment);
        course.CurrentEnrollmentCount++;

        return Task.CompletedTask;
    }

    public Task DropCourseAsync(int studentId, int courseId, string semester)
    {
        var enrollment = _enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId && e.Semester == semester && e.IsActive);
        if (enrollment != null)
        {
            enrollment.IsActive = false;
            _dropHistory.Add(enrollment);
            _enrollments.Remove(enrollment);

            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null) course.CurrentEnrollmentCount--;
        }
        return Task.CompletedTask;
    }
}
