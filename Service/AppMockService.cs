using WorkoutManagerNoam.Models;

namespace WorkoutManagerNoam.Service
{
    public class AppMockService : IAppService
    {
        private readonly List<User> _users;

        public AppMockService()
        {
            _users = new List<User>();

            _users.Add(new User
            {
                UEmail = "parent@gmail.com",
                UserPassword = "1234",
                FirstName = "Parent",
                IsParent = true,
                Balance = 0
            });

            _users.Add(new User
            {
                UEmail = "child@gmail.com",
                UserPassword = "1234",
                FirstName = "Child",
                IsParent = false,
                Balance = 150
            });
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public User? SignIn(string email, string password)
        {
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].UEmail == email && _users[i].UserPassword == password)
                    return _users[i];
            }

            return null;
        }

        public bool IsEmailExists(string email)
        {
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].UEmail == email)
                    return true;
            }

            return false;
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void AddBalance(double amount)
        {
            if (AppState.CurrentUser != null)
                AppState.CurrentUser.Balance += amount;
        }
    }
}
