using System;
namespace AcademicWebPortal.Models.Exceptions;

// [SRP] Single Responsibility Principle:
//   This class encapsulates a single, highly specific business rule violation:
//   attempting to delete a student who is currently enrolled.
public class StudentHasActiveEnrollmentsException : Exception
{
    public StudentHasActiveEnrollmentsException() : base("Student cannot be deleted because they have active enrollments.") { }
    public StudentHasActiveEnrollmentsException(string message) : base(message) { }
}
