using System.ComponentModel.DataAnnotations;
namespace AcademicWebPortal.Models;

// [LSP] Liskov Substitution Principle:
//   Student extends Person and fulfils all of Person's contracts (GetRole, Id, FullName, Email),
//   so it can be substituted anywhere a Person is expected without surprises.
//
// [OCP] Open/Closed Principle:
//   Student-specific behaviour (CGPA, Semester, Enrollments) is added by EXTENSION
//   of Person, not by modifying Person itself.
public class Student : Person, IValidatableObject
{
    public string StudentId { get; set; } = string.Empty;
    public string Password { get; set; } = "student123";
    public DateTime EnrollmentDate { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new();
    public int CurrentSemester { get; set; }
    public double CGPA { get; set; }

    // [LSP] Satisfies the Person abstract contract — callers receive "Student"
    //       without needing to know the concrete type.
    public override string GetRole() => "Student";

    // [SRP] Single Responsibility Principle:
    //       Student owns its own validation rules. The class is responsible for
    //       knowing what constitutes a valid Student record, so this logic lives
    //       here rather than being scattered across pages or services.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CurrentSemester < 1 || CurrentSemester > 8)
        {
            yield return new ValidationResult("Semester must be between 1 and 8.", new[] { nameof(CurrentSemester) });
        }
        if (CGPA < 0.0 || CGPA > 4.0)
        {
            yield return new ValidationResult("CGPA must be between 0.0 and 4.0.", new[] { nameof(CGPA) });
        }
        if (string.IsNullOrWhiteSpace(FullName))
        {
            yield return new ValidationResult("Full Name is required.", new[] { nameof(FullName) });
        }
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
        {
            yield return new ValidationResult("A valid email is required.", new[] { nameof(Email) });
        }
    }
}
