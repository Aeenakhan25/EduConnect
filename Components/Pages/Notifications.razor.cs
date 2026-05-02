using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Enums;

namespace AcademicWebPortal.Components.Pages;

public partial class Notifications : IDisposable
{
    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;

    private List<Notification> NotificationsList { get; set; } = new(); // Renamed to avoid name conflict with page

    protected override async Task OnInitializedAsync()
    {
        NotificationService.OnNotificationReceived += HandleUpdate;
        await LoadNotifications();
    }

    private void HandleUpdate(Notification n) => RefreshUI();

    private async void RefreshUI()
    {
        await InvokeAsync(async () => {
            await LoadNotifications();
            StateHasChanged();
        });
    }

    private async Task LoadNotifications()
    {
        if (AuthStateService.CurrentUser != null)
        {
            NotificationsList = await NotificationService.GetNotificationsAsync(AuthStateService.CurrentUser.Id);
        }
    }

    private async Task MarkAsRead(int id)
    {
        await NotificationService.MarkAsReadAsync(id);
    }

    private string GetBadgeClass(NotificationType type) => type switch
    {
        NotificationType.Enrollment => "bg-primary",
        NotificationType.GradePosted => "bg-success",
        NotificationType.Announcement => "bg-info text-dark",
        _ => "bg-secondary"
    };

    public void Dispose()
    {
        NotificationService.OnNotificationReceived -= HandleUpdate;
    }
}
