using WorkoutManagerNoam.Helper;
using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.Views;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly IDBService _db;

        public INavigation Navigation { get; set; }

        private string _firstName = "";
        private string _lastName = "";
        private string _userEmail = "";
        private string _userPassword = "";
        private string _mobile = "";
        private bool _isParent;
        private bool _entryAsPassword = true;
        private string _passwordIconCode = FontHelper.CLOSED_EYE_ICON;
        private bool _isBusy;

        public bool IsParent
        {
            get => _isParent;
            set
            {
                _isParent = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string UserEmail
        {
            get => _userEmail;
            set
            {
                _userEmail = value;
                OnPropertyChanged();
            }
        }

        public string UserPassword
        {
            get => _userPassword;
            set
            {
                _userPassword = value;
                OnPropertyChanged();
            }
        }

        public string Mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                OnPropertyChanged();
            }
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

        public string PasswordIconCode
        {
            get => _passwordIconCode;
            set
            {
                _passwordIconCode = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
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

            if (EntryAsPassword)
                PasswordIconCode = FontHelper.CLOSED_EYE_ICON;
            else
                PasswordIconCode = FontHelper.OPEN_EYE_ICON;
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
                await Application.Current!.MainPage.DisplayAlert("Error", "Please fill all fields", "OK");
                return;
            }

            if (_db.GetUserByEmail(email) != null)
            {
                IsBusy = false;
                await Application.Current!.MainPage.DisplayAlert("Error", "This email already exists", "OK");
                return;
            }

            User? currentUser = AppState.CurrentUser;

            bool parentCreatesChild = currentUser != null &&
                                      currentUser.IsParent &&
                                      !IsParent;

            // לא מאפשרים ליצור משתמש ילד בלי שהורה מחובר
            if (!IsParent && !parentCreatesChild)
            {
                IsBusy = false;
                await Application.Current!.MainPage.DisplayAlert(
                    "Error",
                    "Child account must be created from a parent account",
                    "OK");
                return;
            }

            User newUser = new User
            {
                FirstName = first,
                LastName = last,
                UserEmail = email,
                UserPassword = pass,
                UMobile = mobile,
                IsParent = IsParent,
                Balance = IsParent ? 0 : 100,
                RegDate = DateTime.Now,
                ParentId = 0
            };

            // כאן מתבצע הקישור האמיתי:
            // אם הורה מחובר יוצר ילד, הילד מקבל את ה-ID של ההורה
            if (parentCreatesChild)
            {
                newUser.ParentId = currentUser!.Id;
            }

            _db.AddUser(newUser);

            IsBusy = false;

            // אם הורה יצר ילד — נשארים מחוברים כהורה, לא מעבירים את המשתמש לילד
            if (parentCreatesChild)
            {
                await Application.Current!.MainPage.DisplayAlert(
                    "Success",
                    "Child account created successfully",
                    "OK");

                await Shell.Current.GoToAsync(nameof(ParentHomePage));
                return;
            }

            // הרשמת הורה חדש רגילה
            AppState.CurrentUser = newUser;
            (Application.Current as App)!.CurrentUser = newUser;

            AppShell shell = IPlatformApplication.Current!.Services.GetService(typeof(AppShell)) as AppShell;

            if (shell == null)
                return;

            Application.Current!.MainPage = shell;

            if (newUser.IsParent)
                await Shell.Current.GoToAsync(nameof(ParentHomePage));
            else
                await Shell.Current.GoToAsync(nameof(ChildHomePage));
        }
    }
}