using Microsoft.AspNetCore.Components;

namespace AcademicWebPortal.Components.Shared;

public partial class AlertBox
{
    [Parameter] public string Message { get; set; } = string.Empty;
    [Parameter] public AlertType Type { get; set; } = AlertType.Info;
    [Parameter] public bool ShowIcon { get; set; } = true;

    private bool IsHidden { get; set; }

    private void Dismiss() => IsHidden = true;

    private string GetIcon() => Type switch
    {
        AlertType.Success => "bi-check-circle-fill",
        AlertType.Danger => "bi-exclamation-octagon-fill",
        AlertType.Warning => "bi-exclamation-triangle-fill",
        _ => "bi-info-circle-fill"
    };
}
