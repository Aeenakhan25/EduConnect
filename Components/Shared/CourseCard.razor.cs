using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Enums;

namespace AcademicWebPortal.Components.Shared;

public partial class CourseCard
{
    [Parameter, EditorRequired] public Course Course { get; set; } = default!;
    [Parameter] public bool IsEnrolled { get; set; }
    [Parameter] public bool ShowAdminActions { get; set; }
    [Parameter] public bool ShowEnrollAction { get; set; }
    
    // Using a tuple to handle different action types
    [Parameter] public EventCallback<(int CourseId, string ActionType)> OnAction { get; set; }

    private string GetStatusBadgeClass(EnrollmentStatus status) => status switch
    {
        EnrollmentStatus.Open => "bg-success",
        EnrollmentStatus.AlmostFull => "bg-warning text-dark",
        EnrollmentStatus.Full => "bg-danger",
        _ => "bg-secondary"
    };
}
