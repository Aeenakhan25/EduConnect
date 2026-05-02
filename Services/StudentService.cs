using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Exceptions;
using AcademicWebPortal.Interfaces;
namespace AcademicWebPortal.Services;

// [SRP] Single Responsibility Principle:
//   This service is responsible only for managing the student collection
//   (CRUD operations for students).
public class StudentService : IStudentService
{
    private readonly List<Student> _students = new()
    {
        new Student { Id = 1, FullName = "John Doe", Email = "john@edu.com", Password = "student123", StudentId = "S1001", EnrollmentDate = DateTime.Now.AddYears(-1), CurrentSemester = 3, CGPA = 3.5 },
        new Student { Id = 2, FullName = "Jane Smith", Email = "jane@edu.com", Password = "student123", StudentId = "S1002", EnrollmentDate = DateTime.Now.AddYears(-2), CurrentSemester = 5, CGPA = 3.8 },
        new Student { Id = 3, FullName = "Aeena Khan", Email = "aeena@edu.com", Password = "student123", StudentId = "S1003", EnrollmentDate = DateTime.Now.AddMonths(-6), CurrentSemester = 1, CGPA = 3.2 },
        new Student { Id = 4, FullName = "Adina Shafqat", Email = "adina@edu.com", Password = "student123", StudentId = "S1004", EnrollmentDate = DateTime.Now.AddYears(-1), CurrentSemester = 2, CGPA = 2.9 },
        new Student { Id = 5, FullName = "Hania khan", Email = "hania@edu.com", Password = "student123", StudentId = "S1005", EnrollmentDate = DateTime.Now.AddYears(-3), CurrentSemester = 7, CGPA = 3.9 }
    };

    public Task<List<Student>> GetStudentsAsync() => Task.FromResult(_students);
    public Task<List<Student>> GetAllStudentsAsync() => Task.FromResult(_students);
    public Task<Student?> GetStudentByIdAsync(int id) => Task.FromResult(_students.FirstOrDefault(s => s.Id == id));
    public Task AddStudentAsync(Student student)
    {
        student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
        if (string.IsNullOrEmpty(student.Password)) student.Password = "student123";
        _students.Add(student);
        return Task.CompletedTask;
    }
    public Task UpdateStudentAsync(Student student)
    {
        var existing = _students.FirstOrDefault(s => s.Id == student.Id);
        if (existing != null)
        {
            existing.FullName = student.FullName;
            existing.Email = student.Email;
            existing.Password = student.Password;
            existing.StudentId = student.StudentId;
            existing.CurrentSemester = student.CurrentSemester;
            existing.CGPA = student.CGPA;
        }
        return Task.CompletedTask;
    }
    public Task DeleteStudentAsync(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student != null)
        {
            if (student.Enrollments != null && student.Enrollments.Any(e => e.IsActive))
                throw new StudentHasActiveEnrollmentsException();

            _students.Remove(student);
        }
        return Task.CompletedTask;
    }
}