using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;

namespace AcademicWebPortal.Components.Pages;

public partial class FacultyGrading
{
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private IGradeService GradeService { get; set; } = default!;
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private List<Course> Courses { get; set; } = new();
    private List<Student> EnrolledStudents { get; set; } = new();
    
    private int _selectedCourseId;
    private int SelectedCourseId
    {
        get => _selectedCourseId;
        set
        {
            if (_selectedCourseId != value)
            {
                _selectedCourseId = value;
                _ = LoadEnrolledStudents(); // Fire-and-forget task
            }
        }
    }

    private Dictionary<int, GradeRecord> TempGrades { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Courses = await CourseService.GetAllCoursesAsync();
    }

    private async Task LoadEnrolledStudents()
    {
        if (SelectedCourseId > 0)
        {
            EnrolledStudents = await CourseService.GetEnrolledStudentsAsync(SelectedCourseId);
            TempGrades.Clear();
            foreach (var student in EnrolledStudents)
            {
                TempGrades[student.Id] = new GradeRecord { StudentId = student.Id, CourseId = SelectedCourseId };
            }
        }
        else
        {
            EnrolledStudents.Clear();
        }
        StateHasChanged();
    }

    private GradeRecord GetTempGrade(int studentId)
    {
        if (!TempGrades.ContainsKey(studentId))
            TempGrades[studentId] = new GradeRecord { StudentId = studentId, CourseId = SelectedCourseId };
        return TempGrades[studentId];
    }

    private async Task SubmitGrade(int studentId)
    {
        var grade = TempGrades[studentId];
        
        // Validation logic
        if (grade.Score < 0 || grade.Score > 100) return;

        var (letter, points) = GradeRecord.CalculateGrade(grade.Score);
        grade.LetterGrade = letter;
        grade.Points = points;

        await GradeService.PostGradeAsync(grade);

        // Notify student
        await NotificationService.SendNotificationAsync(new Notification
        {
            RecipientId = studentId,
            Message = $"New grade posted for course ID: {SelectedCourseId}. Grade: {letter}",
            Type = Models.Enums.NotificationType.GradePosted
        });
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
