using AcademicWebPortal.Models.Enums;
namespace AcademicWebPortal.Models;

// [SRP] Single Responsibility Principle:
//   Notification's sole responsibility is to carry notification data
//   (message, timestamp, read status, recipient). Sending/broadcasting logic
//   lives in INotificationService / NotificationService, not here.
public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; }
    public int RecipientId { get; set; }
}
