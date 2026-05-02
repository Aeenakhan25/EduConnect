using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;

namespace AcademicWebPortal.Components.Pages;

public partial class AdminReport
{
    [Inject] private IStudentService StudentService { get; set; } = default!;
    [Inject] private IGradeService GradeService { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;

    private List<Student> StudentsByPerformance { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var students = await StudentService.GetAllStudentsAsync();
        var courses = await CourseService.GetAllCoursesAsync();

        foreach (var student in students)
        {
            var grades = await GradeService.GetGradesForStudentAsync(student.Id);
            
            double totalPoints = 0;
            int totalCredits = 0;

            foreach (var grade in grades)
            {
                var course = courses.FirstOrDefault(c => c.Id == grade.CourseId);
                if (course != null)
                {
                    totalPoints += grade.Points * course.Credits;
                    totalCredits += course.Credits;
                }
            }

            student.CGPA = totalCredits > 0 ? totalPoints / totalCredits : 0;
            StudentsByPerformance.Add(student);
        }

        // Requirement: Sort by CGPA (Descending for high performance first)
        StudentsByPerformance = StudentsByPerformance.OrderByDescending(s => s.CGPA).ToList();
    }
}
