using Microsoft.Maui.Controls;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.Views;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private readonly Page _signInPage;

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            private set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LogoutCommand { get; }
        public ICommand AdminLoginCommand { get; }

        public AppShellViewModel(SignInPage signInPage)
        {
            _signInPage = signInPage;

            RefreshAdminState();

            LogoutCommand = new Command(Logout);
            AdminLoginCommand = new Command(OpenAdminPage);
        }

        public void RefreshAdminState()
        {
            IsAdmin = (Application.Current as App)?.CurrentUser?.IsAdmin ?? false;
        }

        private async void OpenAdminPage(object obj)
        {
            if (IsAdmin)
                await Shell.Current.GoToAsync(nameof(UsersListPage));
        }

        private void Logout(object obj)
        {
            (Application.Current as App)!.CurrentUser = null;

            AppState.IsAdminLoggedIn = false;
            AppState.AdminUser = null;

            RefreshAdminState();

            var signInVm = _signInPage.BindingContext as SignInViewModel;
            signInVm?.Reset();

            Application.Current!.MainPage = new NavigationPage(_signInPage);
        }
    }
}
