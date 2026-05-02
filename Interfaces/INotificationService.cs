using AcademicWebPortal.Models;
namespace AcademicWebPortal.Interfaces;

// [ISP] Interface Segregation Principle:
//   INotificationService is focused purely on notification delivery and state.
//   It keeps notification logic segregated from core academic operations.
//
// [DIP] Dependency Inversion Principle:
//   Components inject INotificationService instead of instantiating
//   NotificationService directly, decoupling the UI from the specific
//   notification delivery mechanism.
public interface INotificationService
{
    event Action<Notification>? OnNotificationReceived;
    Task<List<Notification>> GetNotificationsAsync(int userId);
    Task SendNotificationAsync(Notification notification);
    Task BroadcastNotificationAsync(string message, string? targetRole = null);
    Task MarkAsReadAsync(int notificationId);
    int GetUnreadCount(int userId);
}
