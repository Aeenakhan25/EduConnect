namespace AcademicWebPortal.Models;

// [OCP] Open/Closed Principle:
//   Person is OPEN for extension — new user types (Student, Faculty, Admin) can be
//   added by subclassing without touching this base class.
//   Person is CLOSED for modification — existing behaviour does not need to change
//   every time a new role is introduced.
//
// [LSP] Liskov Substitution Principle:
//   Every concrete subclass (Student, Faculty, Admin) can substitute for Person
//   wherever a Person reference is expected (e.g. IAuthStateService.CurrentUser)
//   without breaking the caller's expectations.
public abstract class Person
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // [LSP] Forces every subclass to provide its own role string, guaranteeing
    //       that callers can always safely call GetRole() on any Person reference.
    public abstract string GetRole();
}
