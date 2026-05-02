using System.ComponentModel.DataAnnotations;
namespace AcademicWebPortal.Models;

// [SRP] Single Responsibility Principle:
//   GradeRecord is responsible for holding grade data AND computing letter grades.
//   Grading logic belongs here, not in a service or Razor page.
public class GradeRecord : IValidatableObject
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    
    [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
    public double Score { get; set; }
    public string LetterGrade { get; set; } = string.Empty;
    public double Points { get; set; }
    public DateTime DatePosted { get; set; }

    public Student? Student { get; set; }
    public Course? Course { get; set; }

    // [SRP] Grade-to-letter conversion belongs to GradeRecord itself — it is
    //       intrinsically part of what a grade record means.
    // [OCP] New grading bands can be added as extra switch arms without
    //       modifying any service or page that uses this method.
    public static (string Grade, double Points) CalculateGrade(double score) => score switch
    {
        >= 85 => ("A", 4.0),
        >= 70 => ("B", 3.0),
        >= 55 => ("C", 2.0),
        >= 45 => ("D", 1.0),
        _ => ("F", 0.0)
    };

    // [SRP] GradeRecord validates itself — validation rules for a grade live
    //       alongside the grade data, not scattered across the UI.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Score < 0 || Score > 100)
        {
            yield return new ValidationResult("Score must be between 0 and 100.", new[] { nameof(Score) });
        }
    }
}
