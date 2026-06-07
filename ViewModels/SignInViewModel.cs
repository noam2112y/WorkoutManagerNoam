using WorkoutManagerNoam.Helper;
using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.Views;
using System;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    // מנהל את הלוגיקה של מסך ההתחברות.
    public class SignInViewModel : ViewModelBase
    {
        // שירות הנתונים של האפליקציה.
        private readonly IDBService _db;

        // מאפשר לקבל מופעים של מסכים ושירותים שנרשמו במערכת.
        private readonly IServiceProvider _sp;

        // שדה התחברות המשמש לאימייל / שם משתמש.
        private string _FirstName = "";

        // סיסמת המשתמש.
        private string _password = "";

        // שמירת בחירת המשתמש באפשרות זכור אותי.
        private bool _isRememberMeChecked;

        // קובע האם שדה הסיסמה יוצג כסיסמה מוסתרת.
        private bool _entryAsPassword = true;

        // קובע האם הודעת התחברות תוצג במסך.
        private bool _signInMessageVisible;

        // טקסט הודעת ההתחברות.
        private string _loginMassage = "";

        // אייקון הצגת / הסתרת סיסמה.
        private string _passIcon = FontHelper.CLOSED_EYE_ICON;

        // אובייקט הניווט של המסך.
        public INavigation Navigation { get; set; }

        // פקודות שהמסך מפעיל דרך Binding.
        public ICommand ShowPasswordCommand { get; }
        public ICommand SignInCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }

        public SignInViewModel(IDBService dbService, IServiceProvider serviceProvider)
        {
            // קבלת השירותים דרך Dependency Injection.
            _db = dbService;
            _sp = serviceProvider;

            // קישור פקודות לפעולות.
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
            // הכפתור פעיל רק כאשר שני השדות מלאים.
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
            // איפוס נתוני המסך.
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
            // החלפה בין הצגת סיסמה לבין הסתרתה.
            EntryAsPassword = !EntryAsPassword;

            PassIcon = EntryAsPassword
                ? FontHelper.CLOSED_EYE_ICON
                : FontHelper.OPEN_EYE_ICON;
        }

        private async Task SignInButtonClick()
        {
            SingInMessageVisible = true;
            LoginMessage = "";

            // בדיקה בסיסית שהמשתמש מילא את כל השדות.
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(UserPassword))
            {
                LoginMessage = "Please fill all fields";
                return;
            }

            // בדיקת קיום משתמש דרך שכבת השירות.
            if (!_db.IsExist(FirstName, UserPassword))
            {
                LoginMessage = "User does not exist";
                AppState.CurrentUser = null;
                return;
            }

            // קבלת המשתמש המחובר לפי אימייל.
            User current = _db.GetUserByEmail(FirstName);

            if (current == null)
            {
                LoginMessage = "User does not exist";
                AppState.CurrentUser = null;
                return;
            }

            // שמירת המשתמש המחובר לשימוש במסכים אחרים.
            AppState.CurrentUser = current;
            (Application.Current as App)!.CurrentUser = current;

            // שמירת משתמש להתחברות אוטומטית אם נבחרה האפשרות זכור אותי.
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

            // ניווט למסך המתאים לפי סוג המשתמש.
            if (current.IsParent)
                await Shell.Current.GoToAsync(nameof(ParentHomePage));
            else
                await Shell.Current.GoToAsync(nameof(ChildHomePage));
        }

        public async Task CheckRememberedUserAsync()
        {
            // קריאת משתמש שנשמר להתחברות אוטומטית.
            string? token = await SecureStorage.Default.GetAsync("current_user_object");

            if (string.IsNullOrEmpty(token))
                return;

            User user = _db.GetUserByEmail(token);

            if (user == null)
            {
                SecureStorage.Default.Remove("current_user_object");
                return;
            }

            // שחזור המשתמש המחובר.
            AppState.CurrentUser = user;
            (Application.Current as App)!.CurrentUser = user;

            AppShell shell =
                MauiProgram.Services.GetService(typeof(AppShell)) as AppShell;

            if (shell == null)
                return;

            Application.Current!.MainPage = shell;

            // ניווט למסך המתאים לפי סוג המשתמש.
            if (user.IsParent)
                await Shell.Current.GoToAsync(nameof(ParentHomePage));
            else
                await Shell.Current.GoToAsync(nameof(ChildHomePage));
        }
    }
}