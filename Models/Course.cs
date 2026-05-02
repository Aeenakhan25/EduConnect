using AcademicWebPortal.Models.Enums;

namespace AcademicWebPortal.Models;

// [SRP] Single Responsibility Principle:
//   Course is solely responsible for representing course data and computing its
//   own derived state (enrollment status). No service or page logic bleeds into here.
public class Course
{
    public int Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentEnrollmentCount { get; set; }
    public int FacultyId { get; set; }
    public Faculty? Faculty { get; set; }

    // [OCP] Open/Closed Principle:
    //   Status is a computed property derived from existing fields. If business rules
    //   around capacity thresholds change, this is the ONLY place to update — the rest
    //   of the codebase remains closed to modification.
    // [SRP] The course itself owns the responsibility of determining its own status
    //       rather than pushing that logic into a service or UI component.
    public EnrollmentStatus Status
    {
        get
        {
            if (CurrentEnrollmentCount >= MaxCapacity)
                return EnrollmentStatus.Full;
            
            if (CurrentEnrollmentCount >= MaxCapacity * 0.8)
                return EnrollmentStatus.AlmostFull;
            
            return EnrollmentStatus.Open;
        }
    }
}
