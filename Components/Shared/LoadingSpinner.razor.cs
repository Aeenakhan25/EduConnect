using Microsoft.AspNetCore.Components;

namespace AcademicWebPortal.Components.Shared;

public partial class LoadingSpinner
{
    [Parameter] public bool IsVisible { get; set; } = true;
    [Parameter] public string Label { get; set; } = "Loading...";
}
