using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;

namespace AcademicWebPortal.Components.Pages;

public partial class Login
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private string Email { get; set; } = string.Empty;
    private string Password { get; set; } = string.Empty;
    private string? ErrorMessage { get; set; }
    private bool IsLoading { get; set; }

    private async Task HandleLogin()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Please enter your email.";
            return;
        }

        IsLoading = true;
        ErrorMessage = null;

        try
        {
            // Simulate network delay
            await Task.Delay(500);

            var success = await AuthStateService.AuthenticateAsync(Email, Password);
            if (success)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ErrorMessage = "Invalid credentials. Try admin@edu.com, faculty@edu.com, or student@edu.com";
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}
