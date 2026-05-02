using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Enums;
using AcademicWebPortal.Models.Exceptions;

namespace AcademicWebPortal.Components.Pages;

public partial class CourseList
{
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private List<Course> Courses { get; set; } = new();
    private List<int> EnrolledCourseIds { get; set; } = new();
    private string? GeneralError { get; set; }

    private bool IsAdmin => AuthStateService.CurrentUser?.GetRole() == "Admin";
    private bool IsStudent => AuthStateService.CurrentUser?.GetRole() == "Student";

    private bool ShowModal { get; set; }
    private bool IsEditing { get; set; }
    private Course CurrentCourse { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        Courses = await CourseService.GetAllCoursesAsync();
        
        if (IsStudent && AuthStateService.CurrentUser != null)
        {
            var studentId = AuthStateService.CurrentUser.Id;
            EnrolledCourseIds = new List<int>();
            foreach (var course in Courses)
            {
                if (await CourseService.IsStudentEnrolledAsync(studentId, course.Id, "Spring 2024"))
                {
                    EnrolledCourseIds.Add(course.Id);
                }
            }
        }
    }

    private void OpenAddModal()
    {
        CurrentCourse = new Course { MaxCapacity = 30, Credits = 3 };
        IsEditing = false;
        ShowModal = true;
    }

    private async Task HandleCourseAction((int CourseId, string ActionType) args)
    {
        var (courseId, actionType) = args;
        var course = Courses.FirstOrDefault(c => c.Id == courseId);

        switch (actionType)
        {
            case "Enroll":
                await HandleEnroll(courseId);
                break;
            case "Drop":
                await HandleDrop(courseId);
                break;
            case "Edit":
                if (course != null) OpenEditModal(course);
                break;
            case "Delete":
                if (course != null) await ConfirmDelete(course);
                break;
        }
    }

    private void OpenEditModal(Course course)
    {
        CurrentCourse = new Course
        {
            Id = course.Id,
            Title = course.Title,
            CourseCode = course.CourseCode,
            Credits = course.Credits,
            MaxCapacity = course.MaxCapacity,
            Description = course.Description
        };
        IsEditing = true;
        ShowModal = true;
    }

    private void CloseModal() => ShowModal = false;

    private async Task HandleSubmit()
    {
        if (IsEditing) await CourseService.UpdateCourseAsync(CurrentCourse);
        else await CourseService.AddCourseAsync(CurrentCourse);
        
        await LoadData();
        CloseModal();
    }

    private async Task ConfirmDelete(Course course)
    {
        await CourseService.DeleteCourseAsync(course.Id);
        await LoadData();
    }

    private async Task HandleEnroll(int courseId)
    {
        GeneralError = null;
        try
        {
            var studentId = AuthStateService.CurrentUser!.Id;
            await CourseService.EnrollStudentAsync(studentId, courseId, "Spring 2024");
            
            await NotificationService.SendNotificationAsync(new Notification
            {
                RecipientId = studentId,
                Message = $"Successfully enrolled in course ID: {courseId}",
                Type = NotificationType.Enrollment
            });

            await LoadData();
        }
        catch (CourseFullException ex)
        {
            GeneralError = ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            GeneralError = ex.Message;
        }
    }

    private async Task HandleDrop(int courseId)
    {
        GeneralError = null;
        var studentId = AuthStateService.CurrentUser!.Id;
        await CourseService.DropCourseAsync(studentId, courseId, "Spring 2024");
        await LoadData();
    }
}
