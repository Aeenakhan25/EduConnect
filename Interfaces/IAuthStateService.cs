using AcademicWebPortal.Models;
namespace AcademicWebPortal.Interfaces;

// [ISP] Interface Segregation Principle:
//   IAuthStateService exposes ONLY authentication-related members (login, logout,
//   current user, auth-state event). Components that only need to know whether a
//   user is logged in are not forced to depend on unrelated methods.
//
// [DIP] Dependency Inversion Principle:
//   High-level components (MainLayout, NavMenu, Login page) depend on this
//   abstraction, not on the concrete AuthStateService class. The concrete
//   implementation is injected at runtime via the DI container in Program.cs.
public interface IAuthStateService
{
    event Action? OnAuthStateChanged;
    Person? CurrentUser { get; }
    bool IsLoggedIn { get; }
    Task<bool> AuthenticateAsync(string email, string password);
    void Login(Person user);
    void Logout();
}
