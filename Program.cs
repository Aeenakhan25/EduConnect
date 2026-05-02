using AcademicWebPortal.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// [DIP] Dependency Inversion Principle / IoC Container:
//   Here we map abstractions (Interfaces) to their concrete implementations.
//   The rest of the application ONLY depends on the interfaces, so we can swap
//   these services out (e.g. replacing StudentService with SqlStudentService)
//   without changing any UI code.
builder.Services.AddScoped<AcademicWebPortal.Interfaces.IAuthStateService, AcademicWebPortal.Services.AuthStateService>();
builder.Services.AddSingleton<AcademicWebPortal.Interfaces.INotificationService, AcademicWebPortal.Services.NotificationService>();
builder.Services.AddSingleton<AcademicWebPortal.Interfaces.IStudentService, AcademicWebPortal.Services.StudentService>();
builder.Services.AddSingleton<AcademicWebPortal.Interfaces.ICourseService, AcademicWebPortal.Services.CourseService>();
builder.Services.AddSingleton<AcademicWebPortal.Interfaces.IGradeService, AcademicWebPortal.Services.GradeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
