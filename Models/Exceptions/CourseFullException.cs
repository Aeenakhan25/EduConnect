using System;
namespace AcademicWebPortal.Models.Exceptions;

// [SRP] Single Responsibility Principle:
//   This class has exactly one reason to exist: to represent a domain-specific
//   error when a course reaches maximum capacity.
public class CourseFullException : Exception
{
    public CourseFullException() : base("Course has reached its maximum capacity.") { }
    public CourseFullException(string message) : base(message) { }
}
