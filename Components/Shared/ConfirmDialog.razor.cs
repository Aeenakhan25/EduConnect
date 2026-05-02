using Microsoft.AspNetCore.Components;

namespace AcademicWebPortal.Components.Shared;

public partial class ConfirmDialog
{
    [Parameter] public string Title { get; set; } = "Confirm Action";
    [Parameter] public string Message { get; set; } = "Are you sure you want to proceed?";
    [Parameter] public bool IsDanger { get; set; } = true;
    [Parameter] public bool IsVisible { get; set; }
    
    [Parameter] public EventCallback<bool> OnConfirmation { get; set; }

    private string HeaderClass => IsDanger ? "bg-danger" : "bg-primary";
    private string ButtonClass => IsDanger ? "btn-danger" : "btn-primary";

    private async Task Confirm() => await OnConfirmation.InvokeAsync(true);
    private async Task Cancel() => await OnConfirmation.InvokeAsync(false);
}
