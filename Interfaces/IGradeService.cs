using AcademicWebPortal.Models;

namespace AcademicWebPortal.Interfaces;

// [ISP] Interface Segregation Principle:
//   IGradeService contains only methods relevant to grading. If a component
//   needs to display a student's grades, it doesn't have to implement or know
//   about the entire ICourseService or IStudentService.
//
// [DIP] Dependency Inversion Principle:
//   Consumers depend on this abstraction, enabling easy mocking for tests or
//   replacing the implementation without rewriting consumer code.
public interface IGradeService
{
    Task<List<GradeRecord>> GetGradesForStudentAsync(int studentId);
    Task PostGradeAsync(GradeRecord grade);
}
