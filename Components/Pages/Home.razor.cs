using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;

namespace AcademicWebPortal.Components.Pages;

public partial class Home
{
    [Inject] public IAuthStateService AuthStateService { get; set; } = default!;
}
