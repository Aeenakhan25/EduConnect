using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;

namespace AcademicWebPortal.Components.Pages;

public partial class StudentGrades
{
    [Inject] private IGradeService GradeService { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;

    private List<GradeRecord> Grades { get; set; } = new();
    private double CalculatedCGPA { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var studentId = AuthStateService.CurrentUser!.Id;
        var rawGrades = await GradeService.GetGradesForStudentAsync(studentId);
        var courses = await CourseService.GetAllCoursesAsync();

        // Attach course details and calculate CGPA
        double totalPoints = 0;
        int totalCredits = 0;

        foreach (var grade in rawGrades)
        {
            grade.Course = courses.FirstOrDefault(c => c.Id == grade.CourseId);
            if (grade.Course != null)
            {
                totalPoints += grade.Points * grade.Course.Credits;
                totalCredits += grade.Course.Credits;
            }
            Grades.Add(grade);
        }

        CalculatedCGPA = totalCredits > 0 ? totalPoints / totalCredits : 0;
    }

    private string GetGradeColor(string grade) => grade switch
    {
        "A" => "bg-success",
        "B" => "bg-primary",
        "C" => "bg-info text-dark",
        "D" => "bg-warning text-dark",
        _ => "bg-danger"
    };
}
