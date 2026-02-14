using WorkoutManagerNoam.Helper;
using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.Views;
using System;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly IDBService _db;
        private readonly IServiceProvider _sp;

        private string _userName;
        private string _password;
        private bool _isRememberMeChecked;
        private bool _entryAsPassword = true;
        private bool _signInMessageVisible = false;
        private string _loginMassage;

        private string _passIcon = FontHelper.CLOSED_EYE_ICON;

        public INavigation Navigation { get; set; }

        public ICommand ShowPasswordCommand { get; }
        public ICommand SignInCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }

        public SignInViewModel(IDBService dbService, IServiceProvider serviceProvider)
        {
            _db = dbService;
            _sp = serviceProvider;

            ShowPasswordCommand = new Command(TogglePasswordButton);
            SignInCommand = new Command(SignInButtonClick);

            NavigateToSignUpCommand = new Command(async () =>
            {
                if (Navigation == null)
                    return;

                SignUpPage page = _sp.GetService(typeof(SignUpPage)) as SignUpPage;
                await Navigation.PushAsync(page);
            });
        }

        public bool EntryAsPassword
        {
            get => _entryAsPassword;
            set { _entryAsPassword = value; OnPropertyChanged(); }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSignInButtonEnabled));
            }
        }

        public string UserPassword
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSignInButtonEnabled));
            }
        }

        public bool RememberMeChecked
        {
            get { return _isRememberMeChecked; }
            set
            {
                if (_isRememberMeChecked != value)
                {
                    _isRememberMeChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSignInButtonEnabled
        {
            get => !(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPassword));
        }

        public bool SingInMessageVisible
        {
            get => _signInMessageVisible;
            set { _signInMessageVisible = value; OnPropertyChanged(); }
        }

        public string LoginMessage
        {
            get => _loginMassage;
            set { _loginMassage = value; OnPropertyChanged(); }
        }

        public string PassIcon
        {
            get => _passIcon;
            set { _passIcon = value; OnPropertyChanged(); }
        }

        public void Reset()
        {
            UserName = "";
            UserPassword = "";
            LoginMessage = "";
            SingInMessageVisible = false;
            EntryAsPassword = true;
            PassIcon = FontHelper.CLOSED_EYE_ICON;
        }

        private void TogglePasswordButton()
        {
            EntryAsPassword = !EntryAsPassword;
            PassIcon = EntryAsPassword ? FontHelper.CLOSED_EYE_ICON : FontHelper.OPEN_EYE_ICON;
        }

        private void SignInButtonClick()
        {
            SingInMessageVisible = true;

            if (!_db.IsExist(UserName, UserPassword))
            {
                LoginMessage = "user not exist";
                AppState.IsAdminLoggedIn = false;
                AppState.AdminUser = null;
                return;
            }

            // ✅ להביא את המשתמש פעם אחת
            User current = _db.GetUserByEmail(UserName);
            (Application.Current as App)!.CurrentUser = current;

            // ✅ הכי חשוב: להרים/להוריד את מצב האדמין
            if (current != null && current.IsAdmin)
            {
                AppState.IsAdminLoggedIn = true;
                AppState.AdminUser = current;
            }
            else
            {
                AppState.IsAdminLoggedIn = false;
                AppState.AdminUser = null;
            }

            // ✅ Remember me
            if (RememberMeChecked)
            {
                SecureStorage.Default.SetAsync("current_user_object", UserName);
            }
            else
            {
                SecureStorage.Default.Remove("current_user_object");
            }

            // לעדכן VM של ה-Shell כדי שיראו כפתורי Admin/Logout נכון
            AppShellViewModel shellVm =
                IPlatformApplication.Current!.Services.GetService(typeof(AppShellViewModel)) as AppShellViewModel;
            shellVm?.RefreshAdminState();

            AppShell shell =
                IPlatformApplication.Current!.Services.GetService(typeof(AppShell)) as AppShell;

            Application.Current!.MainPage = shell;
        }
        /*
        internal async void OnAppearing()
        {
            // chack if user exist in storage
            string? token = await SecureStorage.Default.GetAsync("current_user_object");
            if (!string.IsNullOrEmpty(token))
            {
                User user = _db.GetUserByEmail(token);
                if (user != null)
                {
                    // set current user 
                    (App.Current as App)!.CurrentUser = user;
                    //navigte to main page of shell
                    var mainPage = IPlatformApplication.Current!.Services.GetService<AppShell>();
                    Application.Current!.Windows[0].Page = mainPage;
                }
            }

        }
        */
    }
}
