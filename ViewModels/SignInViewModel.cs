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

        private string _FirstName = "";
        private string _password = "";
        private bool _isRememberMeChecked;
        private bool _entryAsPassword = true;
        private bool _signInMessageVisible;
        private string _loginMassage = "";

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
            SignInCommand = new Command(async () => await SignInButtonClick());

            NavigateToSignUpCommand = new Command(async () =>
            {
                if (Navigation == null)
                    return;

                SignUpPage page = _sp.GetService(typeof(SignUpPage)) as SignUpPage;
                if (page != null)
                    await Navigation.PushAsync(page);
            });
        }

        public bool EntryAsPassword
        {
            get => _entryAsPassword;
            set
            {
                _entryAsPassword = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => _FirstName;
            set
            {
                _FirstName = value;
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
            get => _isRememberMeChecked;
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
            get => !(string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(UserPassword));
        }

        public bool SingInMessageVisible
        {
            get => _signInMessageVisible;
            set
            {
                _signInMessageVisible = value;
                OnPropertyChanged();
            }
        }

        public string LoginMessage
        {
            get => _loginMassage;
            set
            {
                _loginMassage = value;
                OnPropertyChanged();
            }
        }

        public string PassIcon
        {
            get => _passIcon;
            set
            {
                _passIcon = value;
                OnPropertyChanged();
            }
        }

        public void Reset()
        {
            FirstName = "";
            UserPassword = "";
            LoginMessage = "";
            SingInMessageVisible = false;
            EntryAsPassword = true;
            PassIcon = FontHelper.CLOSED_EYE_ICON;
            RememberMeChecked = false;
        }

        private void TogglePasswordButton()
        {
            EntryAsPassword = !EntryAsPassword;
            PassIcon = EntryAsPassword ? FontHelper.CLOSED_EYE_ICON : FontHelper.OPEN_EYE_ICON;
        }

        private async Task SignInButtonClick()
        {
            SingInMessageVisible = true;
            LoginMessage = "";

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(UserPassword))
            {
                LoginMessage = "Please fill all fields";
                return;
            }

            if (!_db.IsExist(FirstName, UserPassword))
            {
                LoginMessage = "User does not exist";
                AppState.CurrentUser = null;
                return;
            }

            User current = _db.GetUserByEmail(FirstName);

            if (current == null)
            {
                LoginMessage = "User does not exist";
                AppState.CurrentUser = null;
                return;
            }

            AppState.CurrentUser = current;
            (Application.Current as App)!.CurrentUser = current;

            if (RememberMeChecked)
            {
                await SecureStorage.Default.SetAsync("current_user_object", FirstName);
            }
            else
            {
                SecureStorage.Default.Remove("current_user_object");
            }

            AppShell shell =
                IPlatformApplication.Current!.Services.GetService(typeof(AppShell)) as AppShell;

            if (shell == null)
                return;

            Application.Current!.MainPage = shell;

            if (current.IsParent)
                await Shell.Current.GoToAsync(nameof(ParentHomePage));
            else
                await Shell.Current.GoToAsync(nameof(ChildHomePage));
        }

        internal async void OnAppearing()
        {
            string? token = await SecureStorage.Default.GetAsync("current_user_object");

            if (string.IsNullOrEmpty(token))
                return;

            User user = _db.GetUserByEmail(token);

            if (user == null)
                return;

            AppState.CurrentUser = user;
            (App.Current as App)!.CurrentUser = user;

            AppShell shell =
                IPlatformApplication.Current!.Services.GetService(typeof(AppShell)) as AppShell;

            if (shell == null)
                return;

            Application.Current!.MainPage = shell;

            if (user.IsParent)
                await Shell.Current.GoToAsync(nameof(ParentHomePage));
            else
                await Shell.Current.GoToAsync(nameof(ChildHomePage));
        }
    }
}