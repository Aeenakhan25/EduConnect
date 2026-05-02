using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Enums;

namespace AcademicWebPortal.Components.Layout;

public partial class NotificationBell : IDisposable
{
    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;

    private List<Notification> Notifications { get; set; } = new();
    private int UnreadCount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // 🚨 REQUIREMENT: Subscribe to specific event Action<Notification>
        NotificationService.OnNotificationReceived += HandleNotification;
        AuthStateService.OnAuthStateChanged += RefreshNotifications;
        
        await LoadNotifications();
    }

    private void HandleNotification(Notification notification)
    {
        // Only react if the notification is for the current user
        if (AuthStateService.CurrentUser != null && notification.RecipientId == AuthStateService.CurrentUser.Id)
        {
            RefreshNotifications();
        }
    }

    private async void RefreshNotifications()
    {
        await InvokeAsync(async () =>
        {
            await LoadNotifications();
            StateHasChanged();
        });
    }

    private async Task LoadNotifications()
    {
        if (AuthStateService.CurrentUser != null)
        {
            Notifications = await NotificationService.GetNotificationsAsync(AuthStateService.CurrentUser.Id);
            UnreadCount = NotificationService.GetUnreadCount(AuthStateService.CurrentUser.Id);
        }
        else
        {
            Notifications.Clear();
            UnreadCount = 0;
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
        // 🚨 REQUIREMENT: Unsubscribe
        NotificationService.OnNotificationReceived -= HandleNotification;
        AuthStateService.OnAuthStateChanged -= RefreshNotifications;
    }
}
