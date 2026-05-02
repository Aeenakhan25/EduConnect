using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;

namespace AcademicWebPortal.Components.Pages;

public partial class StudentDetail
{
    [Parameter] public int Id { get; set; }
    
    [Inject] private IStudentService StudentService { get; set; } = default!;
    [Inject] private IGradeService GradeService { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private Student? Student { get; set; }
    private List<AcademicRecordViewModel> CourseRecords { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Student = await StudentService.GetStudentByIdAsync(Id);
        
        if (Student != null)
        {
            var grades = await GradeService.GetGradesForStudentAsync(Id);
            var courses = await CourseService.GetAllCoursesAsync();

            // Requirement: Sort courses alphabetically
            CourseRecords = grades
                .Select(g => {
                    var course = courses.FirstOrDefault(c => c.Id == g.CourseId);
                    return new AcademicRecordViewModel
                    {
                        Code = course?.CourseCode ?? "N/A",
                        Title = course?.Title ?? "Unknown Course",
                        Semester = "N/A", // In a real app, this would come from Enrollment
                        LetterGrade = g.LetterGrade,
                        Score = g.Score
                    };
                })
                .OrderBy(r => r.Title)
                .ToList();
        }
    }

    private void GoBack()
    {
        NavigationManager.NavigateTo("/admin/students");
    }

    private class AcademicRecordViewModel
    {
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public string LetterGrade { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}
