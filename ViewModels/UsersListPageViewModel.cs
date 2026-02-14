using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    public class UsersListPageViewModel : ViewModelBase
    {
        private readonly IDBService _db;

        public ObservableCollection<ObservableUser> AllUsers { get; private set; }
        private readonly List<ObservableUser> _allUsersSource;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public ICommand GetAllUsersCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ViewAccountPage { get; }   // ✅ לפתיחת דף פרטים/עריכה

        public UsersListPageViewModel(IDBService dbService)
        {
            _db = dbService;

            AllUsers = new ObservableCollection<ObservableUser>();
            _allUsersSource = new List<ObservableUser>();

            GetAllUsersCommand = new Command(async () => await LoadUsers());
            SelectionChangedCommand = new Command<SelectionChangedEventArgs>(OnSelectionChanged);

            AddUserCommand = new Command(OnAddUser);
            DeleteUserCommand = new Command<ObservableUser>(OnDeleteUser);

            // ✅ חשוב: זה מה שמחובר ל-Swipe "צפייה" וגם ללחיצה על השורה (Tap)
            ViewAccountPage = new Command<ObservableUser>(OnViewUser);

            _ = LoadUsers();
        }

        private async Task LoadUsers()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            List<User> list = _db.GetAllUsers();

            _allUsersSource.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                User u = list[i];
                ObservableUser ou = new ObservableUser(u);
                _allUsersSource.Add(ou);
            }

            ApplyFilter();

            IsBusy = false;
            await Task.CompletedTask;
        }

        private void ApplyFilter()
        {
            string s = (SearchText ?? "").Trim().ToLower();

            AllUsers.Clear();

            if (s == "")
            {
                for (int i = 0; i < _allUsersSource.Count; i++)
                    AllUsers.Add(_allUsersSource[i]);
                return;
            }

            for (int i = 0; i < _allUsersSource.Count; i++)
            {
                ObservableUser u = _allUsersSource[i];

                string first = (u.FirstName ?? "").ToLower();
                string last = (u.LastName ?? "").ToLower();
                string email = (u.UEmail ?? "").ToLower();

                if (first.Contains(s) || last.Contains(s) || email.Contains(s))
                    AllUsers.Add(u);
            }
        }

        // בחירה מהרשימה
        private async void OnSelectionChanged(SelectionChangedEventArgs args)
        {
            if (args == null || args.CurrentSelection == null || args.CurrentSelection.Count == 0)
                return;

            ObservableUser user = args.CurrentSelection[0] as ObservableUser;
            if (user == null)
                return;

            // ✅ נשתמש באותה פעולה כמו "צפייה"
            OnViewUser(user);
        }

        // ✅ פעולה אחת מרכזית שפותחת דף עריכה
        private async void OnViewUser(ObservableUser user)
        {
            if (user == null) return;
            if (!AppState.IsAdminLoggedIn) return;

            AppState.SelectedUser = user;
            await Shell.Current.GoToAsync(nameof(UserDetailsPage));
        }

        private void OnAddUser()
        {
            if (!AppState.IsAdminLoggedIn)
                return;

            User newUser = new User
            {
                FirstName = "New",
                LastName = "User",
                UserEmail = "new" + DateTime.Now.Ticks + "@gmail.com",
                UserPassword = "1234",
                UMobile = "0500000000",
                RegDate = DateTime.Now,
                IsAdmin = false
            };

            _db.AddUser(newUser);

            ObservableUser ou = new ObservableUser(newUser);
            _allUsersSource.Add(ou);
            ApplyFilter();
        }

        private void OnDeleteUser(ObservableUser user)
        {
            if (user == null)
                return;

            if (!AppState.IsAdminLoggedIn)
                return;

            _db.RemoveUser(new User { Id = user.Id });

            for (int i = 0; i < _allUsersSource.Count; i++)
            {
                if (_allUsersSource[i].Id == user.Id)
                {
                    _allUsersSource.RemoveAt(i);
                    break;
                }
            }

            ApplyFilter();
        }
    }
}
