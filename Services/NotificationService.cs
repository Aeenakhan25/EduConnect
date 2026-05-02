using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Enums;
using AcademicWebPortal.Interfaces;

namespace AcademicWebPortal.Services;

// [SRP] Single Responsibility Principle:
//   This service handles the state, broadcasting, and delivery mechanics of
//   notifications, and absolutely nothing else.
public class NotificationService : INotificationService
{
    // Using a lock object to prevent errors if two things happen at the same exact time
    private readonly List<Notification> _notifications = new();
    private readonly object _lock = new();
    private readonly IStudentService _studentService;

    public event Action<Notification>? OnNotificationReceived;

    // [DIP] Dependency Inversion Principle:
    //   Injects IStudentService rather than depending on a concrete implementation,
    //   allowing it to safely resolve students when broadcasting "All" or "Student" roles.
    public NotificationService(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public Task<List<Notification>> GetNotificationsAsync(int userId)
    {
        lock (_lock)
        {
            return Task.FromResult(_notifications
                .Where(n => n.RecipientId == userId)
                .OrderByDescending(n => n.Timestamp)
                .ToList());
        }
    }

    public Task SendNotificationAsync(Notification notification)
    {
        lock (_lock)
        {
            notification.Id = _notifications.Count + 1;
            notification.Timestamp = DateTime.Now;
            _notifications.Add(notification);
        }

        OnNotificationReceived?.Invoke(notification);
        return Task.CompletedTask;
    }

    public async Task BroadcastNotificationAsync(string message, string? targetRole = null)
    {
        var recipients = new List<int>();

        // 1. Determine recipients
        if (targetRole == "All" || targetRole == "Student")
        {
            var students = await _studentService.GetAllStudentsAsync();
            recipients.AddRange(students.Select(s => s.Id));
        }

        if (targetRole == "All" || targetRole == "Faculty")
        {
            // Assuming 200 is your hardcoded Faculty ID for testing
            recipients.Add(200);
        }

        // 2. Create and store notifications
        foreach (var recipientId in recipients.Distinct())
        {
            var note = new Notification
            {
                RecipientId = recipientId,
                Message = message,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            lock (_lock)
            {
                note.Id = _notifications.Count + 1;
                _notifications.Add(note);
            }

            // This triggers the Toast in MainLayout and the Red Badge in NavMenu
            OnNotificationReceived?.Invoke(note);
        }
    }

    public Task MarkAsReadAsync(int notificationId)
    {
        lock (_lock)
        {
            var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                // Invoke event so the badge count updates immediately
                OnNotificationReceived?.Invoke(notification);
            }
        }
        return Task.CompletedTask;
    }

    public int GetUnreadCount(int userId)
    {
        lock (_lock)
        {
            return _notifications.Count(n => n.RecipientId == userId && !n.IsRead);
        }
    }

    // New helper to clear notifications for a specific user
    public void ClearAllForUser(int userId)
    {
        lock (_lock)
        {
            _notifications.RemoveAll(n => n.RecipientId == userId);
        }
        // Send a dummy notification to trigger a UI refresh
        OnNotificationReceived?.Invoke(new Notification { RecipientId = userId });
    }
}