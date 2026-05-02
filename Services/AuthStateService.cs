using AcademicWebPortal.Models;
using AcademicWebPortal.Interfaces;
namespace AcademicWebPortal.Services;

// [SRP] Single Responsibility Principle:
//   This service only handles authentication state (login/logout, current user).
//   It doesn't handle course logic or notifications.
// [DIP] Dependency Inversion Principle:
//   It relies on the IStudentService abstraction instead of the concrete
//   StudentService class to fetch students for login.
public class AuthStateService : IAuthStateService
{
    public event Action? OnAuthStateChanged;
    public Person? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;
    private readonly IStudentService _studentService;
    private readonly List<Person> _staffUsers = new()
    {
        new Admin { Id = 100, FullName = "Admin User", Email = "admin@edu.com" },
        new Faculty { Id = 200, FullName = "Prof. Smith", Email = "faculty@edu.com", Department = "Computer Science" }
    };

    public AuthStateService(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task<bool> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var cleanEmail = email.Trim().ToLower();

        // Check Staff
        var staff = _staffUsers.FirstOrDefault(u => u.Email.Trim().ToLower() == cleanEmail);
        if (staff != null && password == "admin123")
        {
            Login(staff);
            return true;
        }

        //  Check Students
        var allStudents = await _studentService.GetAllStudentsAsync();
        var student = allStudents.FirstOrDefault(s =>
            s.Email.Trim().ToLower() == cleanEmail &&
            s.Password == "student123"); 
        if (student != null)
        {
            Login(student);
            return true;
        }
        return false;
    }

    public void Login(Person user)
    {
        CurrentUser = user;
        NotifyStateChanged();
    }
    public void Logout()
    {
        CurrentUser = null;
        NotifyStateChanged();
    }
    private void NotifyStateChanged() => OnAuthStateChanged?.Invoke();
}