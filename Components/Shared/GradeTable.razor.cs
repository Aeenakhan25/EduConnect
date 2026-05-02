using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Models;

namespace AcademicWebPortal.Components.Shared;

public partial class GradeTable
{
    [Parameter] public List<GradeRecord> Grades { get; set; } = new();
    [Parameter] public bool ShowStudentName { get; set; }
    [Parameter] public bool ShowCourseName { get; set; }

    private string GetGradeColor(string grade) => grade switch
    {
        "A" => "bg-success",
        "B" => "bg-primary",
        "C" => "bg-info text-dark",
        "D" => "bg-warning text-dark",
        _ => "bg-danger"
    };
}
