using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
namespace AcademicWebPortal.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        AuthStateService.OnAuthStateChanged += HandleAuthStateChanged;
        CheckAuthorization();
    }
    private void HandleAuthStateChanged()
    {
        InvokeAsync(StateHasChanged);
    }
    private void CheckAuthorization()
    {
        var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        if (!AuthStateService.IsLoggedIn && !relativePath.StartsWith("login", StringComparison.OrdinalIgnoreCase))
        {
            NavigationManager.NavigateTo("/login");
        }
    }

    public void Dispose()
    {
        AuthStateService.OnAuthStateChanged -= HandleAuthStateChanged;
    }
}