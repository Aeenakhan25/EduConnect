using AcademicWebPortal.Models;
namespace AcademicWebPortal.Interfaces;

// [ISP] Interface Segregation Principle: 
//   Specific contract for student operations. It limits its scope strictly to
//   managing student entities, separate from courses and grades.
//
// [DIP] Dependency Inversion Principle:
//   High-level modules rely on this abstraction instead of the low-level 
//   StudentService implementation.
public interface IStudentService
{
    Task<List<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task AddStudentAsync(Student student);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(int id);
}
