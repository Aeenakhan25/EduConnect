using AcademicWebPortal.Models;
using AcademicWebPortal.Interfaces;
namespace AcademicWebPortal.Services;

// [SRP] Single Responsibility Principle:
//   GradeService manages strictly the storage and retrieval of grades.
//   It is entirely isolated from enrollment rules or authentication logic.
public class GradeService : IGradeService
{
    private readonly List<GradeRecord> _grades = new();

    public Task<List<GradeRecord>> GetGradesForStudentAsync(int studentId)
    {
        return Task.FromResult(_grades.Where(g => g.StudentId == studentId).ToList());
    }
    public Task PostGradeAsync(GradeRecord grade)
    {
        grade.Id = _grades.Count + 1;
        grade.DatePosted = DateTime.Now;
        _grades.Add(grade);
        return Task.CompletedTask;
    }
}
