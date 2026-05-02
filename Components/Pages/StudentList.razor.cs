using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;
using AcademicWebPortal.Models.Exceptions;

namespace AcademicWebPortal.Components.Pages;

public partial class StudentList
{
    [Parameter] public int? Id { get; set; }
    
    [Inject] private IStudentService StudentService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private List<Student> Students { get; set; } = new();
    private string SearchTerm { get; set; } = string.Empty;

    private IEnumerable<Student> FilteredStudents => string.IsNullOrWhiteSpace(SearchTerm) 
        ? Students 
        : Students.Where(s => 
            s.FullName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || 
            s.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || 
            s.StudentId.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));

    private bool ShowModal { get; set; }
    private bool IsEditing { get; set; }
    private Student CurrentStudent { get; set; } = new();

    private bool ShowDeleteModal { get; set; }
    private Student? StudentToDelete { get; set; }
    private string? DeleteError { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadStudents();
    }

    protected override async Task OnParametersSetAsync()
    {
        var uri = NavigationManager.Uri.ToLower();
        if (uri.Contains("/add"))
        {
            OpenAddModal();
        }
        else if (Id.HasValue && uri.Contains("/edit"))
        {
            var student = await StudentService.GetStudentByIdAsync(Id.Value);
            if (student != null) OpenEditModal(student);
        }
    }

    private async Task LoadStudents()
    {
        Students = await StudentService.GetAllStudentsAsync();
    }

    private void OpenAddModal()
    {
        CurrentStudent = new Student { EnrollmentDate = DateTime.Now, CurrentSemester = 1 };
        IsEditing = false;
        ShowModal = true;
    }

    private void OpenEditModal(Student student)
    {
        // Create a copy to avoid instant updates in the list before saving
        CurrentStudent = new Student
        {
            Id = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            StudentId = student.StudentId,
            CurrentSemester = student.CurrentSemester,
            CGPA = student.CGPA,
            EnrollmentDate = student.EnrollmentDate
        };
        IsEditing = true;
        ShowModal = true;
    }

    private void CloseModal()
    {
        ShowModal = false;
    }

    private async Task HandleSubmit()
    {
        if (IsEditing)
        {
            await StudentService.UpdateStudentAsync(CurrentStudent);
        }
        else
        {
            await StudentService.AddStudentAsync(CurrentStudent);
        }
        
        await LoadStudents();
        CloseModal();
    }

    private void ConfirmDelete(Student student)
    {
        StudentToDelete = student;
        DeleteError = null;
        ShowDeleteModal = true;
    }

    private async Task HandleDelete()
    {
        if (StudentToDelete == null) return;

        try
        {
            await StudentService.DeleteStudentAsync(StudentToDelete.Id);
            await LoadStudents();
            ShowDeleteModal = false;
        }
        catch (StudentHasActiveEnrollmentsException ex)
        {
            DeleteError = ex.Message;
        }
        catch (Exception)
        {
            DeleteError = "An unexpected error occurred.";
        }
    }

    private void ViewDetails(int id)
    {
        NavigationManager.NavigateTo($"/admin/students/{id}");
    }
}
