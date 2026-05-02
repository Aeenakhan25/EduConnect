using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;

namespace AcademicWebPortal.Components.Pages;

public partial class AdminNotifications
{
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private string BroadcastMessage { get; set; } = string.Empty;
    private string TargetRole { get; set; } = "All";
    private string? SuccessMessage { get; set; }

    private async Task HandleBroadcast()
    {
        if (string.IsNullOrWhiteSpace(BroadcastMessage)) return;

        string? roleFilter = TargetRole == "All" ? null : TargetRole;
        await NotificationService.BroadcastNotificationAsync(BroadcastMessage, roleFilter);
        
        SuccessMessage = $"Announcement broadcasted to {TargetRole} successfully!";
        BroadcastMessage = string.Empty;
        
        _ = Task.Delay(3000).ContinueWith(_ => {
            SuccessMessage = null;
            InvokeAsync(StateHasChanged);
        });
    }
}
