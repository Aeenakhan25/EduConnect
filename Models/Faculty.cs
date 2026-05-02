namespace AcademicWebPortal.Models;

// [LSP] Liskov Substitution Principle:
//   Faculty fulfils the full Person contract (GetRole, Id, FullName, Email) so it can
//   substitute a Person reference anywhere — e.g. AuthStateService.CurrentUser.
//
// [OCP] Open/Closed Principle:
//   Faculty-specific data (Department, CoursesTaught) is added through extension,
//   keeping the base Person class closed for modification.
public class Faculty : Person
{
    public string Department { get; set; } = string.Empty;
    public List<Course> CoursesTaught { get; set; } = new();

    // [LSP] Fulfils the abstract GetRole() contract from Person so callers
    //       can always resolve the role without knowing the concrete type.
    public override string GetRole() => "Faculty";
}
