using Microsoft.AspNetCore.Components;
using AcademicWebPortal.Interfaces;
using AcademicWebPortal.Models;

namespace AcademicWebPortal.Shared
{
    public class NavMenuBase : ComponentBase, IDisposable
    {
        [Inject]
        public IAuthStateService AuthStateService { get; set; } = default!;

        [Inject]
        public INotificationService NotificationService { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        protected override void OnInitialized()
        {
            // Register Observers
            AuthStateService.OnAuthStateChanged += HandleRefresh;
            NotificationService.OnNotificationReceived += HandleNotificationUpdate;
        }

        private void HandleRefresh() => InvokeAsync(StateHasChanged);

        private void HandleNotificationUpdate(Notification note)
        {
            // Only refresh the sidebar if the message is for the logged-in user
            if (AuthStateService.IsLoggedIn && note.RecipientId == AuthStateService.CurrentUser?.Id)
            {
                InvokeAsync(StateHasChanged);
            }
        }

        public void HandleLogout()
        {
            AuthStateService.Logout();
            NavigationManager.NavigateTo("/login");
        }

        public void Dispose()
        {
            // Unsubscribe to prevent memory leaks
            AuthStateService.OnAuthStateChanged -= HandleRefresh;
            NotificationService.OnNotificationReceived -= HandleNotificationUpdate;
        }
    }
}