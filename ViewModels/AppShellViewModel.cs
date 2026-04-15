using Microsoft.Maui.Controls;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.Views;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private readonly Page _signInPage;

        private bool _IsParent;
        public bool IsParent
        {
            get => _IsParent;
            private set
            {
                if (_IsParent != value)
                {
                    _IsParent = value;
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
        }

        public void RefreshAdminState()
        {
            IsParent = (Application.Current as App)?.CurrentUser?.IsParent ?? false;
        }

        

        private void Logout(object obj)
        {
            (Application.Current as App)!.CurrentUser = null;

            //AppState.IsParentLoggedIn = false;
            //AppState.AdminUser = null;

            RefreshAdminState();

            var signInVm = _signInPage.BindingContext as SignInViewModel;
            signInVm?.Reset();

            Application.Current!.MainPage = new NavigationPage(_signInPage);
        }
    }
}
