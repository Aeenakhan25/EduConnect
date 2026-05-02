using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Models;

namespace AcademicWebPortal.Components.Shared;

public partial class StudentCard
{
    [Parameter, EditorRequired] public Student Student { get; set; } = default!;
    [Parameter] public EventCallback<int> OnViewDetails { get; set; }

    private async Task HandleViewDetails()
    {
        await OnViewDetails.InvokeAsync(Student.Id);
    }
}
