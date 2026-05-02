namespace AcademicWebPortal.Models;

// [LSP] Liskov Substitution Principle:
//   Admin fulfils the full Person contract, so any code that works with a Person
//   reference will work seamlessly with an Admin instance.
//
// [OCP] Open/Closed Principle:
//   Admin-specific state (OfficeLocation) is added by extending Person,
//   leaving the base class untouched.
public class Admin : Person
{
    public string OfficeLocation { get; set; } = string.Empty;

    // [LSP] Provides the required GetRole() implementation so callers (e.g. NavMenu
    //       role-based navigation) can safely call GetRole() on any Person.
    public override string GetRole() => "Admin";
}
