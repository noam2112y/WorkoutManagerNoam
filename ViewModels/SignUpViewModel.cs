using WorkoutManagerNoam.Helper;
using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly IDBService _db;

        public INavigation Navigation { get; set; }

        private string _firstName;
        private string _lastName;
        private string _userEmail;
        private string _userPassword;
        private string _mobile;

        private bool _entryAsPassword = true;
        private string _passwordIconCode = FontHelper.CLOSED_EYE_ICON;

        private bool _isBusy;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string UserEmail
        {
            get => _userEmail;
            set { _userEmail = value; OnPropertyChanged(); }
        }

        public string UserPassword
        {
            get => _userPassword;
            set { _userPassword = value; OnPropertyChanged(); }
        }

        public string Mobile
        {
            get => _mobile;
            set { _mobile = value; OnPropertyChanged(); }
        }

        public bool EntryAsPassword
        {
            get => _entryAsPassword;
            set { _entryAsPassword = value; OnPropertyChanged(); }
        }

        public string PasswordIconCode
        {
            get => _passwordIconCode;
            set { _passwordIconCode = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand TogglePasswordCommand { get; }
        public ICommand SignUpCommand { get; }
        public ICommand BackToSignInCommand { get; }

        public SignUpViewModel(IDBService dbService)
        {
            _db = dbService;

            TogglePasswordCommand = new Command(TogglePassword);
            SignUpCommand = new Command(async () => await SignUp());
            BackToSignInCommand = new Command(async () => await GoBack());
        }

        private void TogglePassword()
        {
            EntryAsPassword = !EntryAsPassword;
            PasswordIconCode = EntryAsPassword ? FontHelper.CLOSED_EYE_ICON : FontHelper.OPEN_EYE_ICON;
        }

        private async Task GoBack()
        {
            if (Navigation != null && Navigation.NavigationStack.Count > 0)
                await Navigation.PopAsync();
        }

        private async Task SignUp()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            string first = (FirstName ?? "").Trim();
            string last = (LastName ?? "").Trim();
            string email = (UserEmail ?? "").Trim();
            string pass = (UserPassword ?? "").Trim();
            string mobile = (Mobile ?? "").Trim();

            if (first == "" || last == "" || email == "" || pass == "" || mobile == "")
            {
                IsBusy = false;
                await Application.Current!.MainPage.DisplayAlert("שגיאה", "מלא/י את כל השדות", "OK");
                return;
            }

            // אימייל כבר קיים?
            if (_db.GetUserByEmail(email) != null)
            {
                IsBusy = false;
                await Application.Current!.MainPage.DisplayAlert("שגיאה", "האימייל כבר רשום במערכת", "OK");
                return;
            }

            User newUser = new User
            {
                FirstName = first,
                LastName = last,
                UserEmail = email,
                UserPassword = pass,
                UMobile = mobile,
                IsAdmin = false,
                RegDate = DateTime.Now
            };

            _db.AddUser(newUser);

            (Application.Current as App)!.CurrentUser = newUser;

            AppShellViewModel shellVm =
                IPlatformApplication.Current!.Services.GetService(typeof(AppShellViewModel)) as AppShellViewModel;
            shellVm?.RefreshAdminState();

            AppShell shell =
                IPlatformApplication.Current!.Services.GetService(typeof(AppShell)) as AppShell;

            IsBusy = false;

            Application.Current!.MainPage = shell;
        }
    }
}
