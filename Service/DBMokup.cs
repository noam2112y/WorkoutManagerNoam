using WorkoutManagerNoam.Models;
using System.Collections.Generic;
using System.Linq;

namespace WorkoutManagerNoam.Service
{
    public interface IDBService
    {
        void AddUser(User user);
        User GetUserByEmail(string uEmail);
        bool IsExist(string uEmail, string uPass);
        void RemoveUser(User user);
        void UpdateUser(User user);

        
        List<User> GetAllUsers();
    }


    public class DBMokup : IDBService
    {
        private static List<User> _users = new List<User>()
        {
            new User { Id=1, FirstName="Noam", LastName="1234567890", UserEmail="Noam", UserPassword="1234567890", UMobile="0500000000", IsAdmin=true, RegDate=System.DateTime.Now },
            new User { Id=2, FirstName="AA", LastName="BB", UserEmail="User1@gmail.com", UserPassword="pass1", UMobile="0501111111", RegDate=System.DateTime.Now },
            new User { Id=3, FirstName="CC", LastName="DD", UserEmail="user2@gmail.com", UserPassword="pass2", UMobile="0502222222", RegDate=System.DateTime.Now }
        };
        public List<User> GetAllUsers()
        {
            return new List<User>(_users);
        }
        public List<User> GetUsers()
        {
            return _users;
        }

        public bool IsExist(string uEmail, string uPass)
        {
            return _users.Any(u => u.UserEmail == uEmail && u.UserPassword == uPass);
        }

        public User GetUserByEmail(string uEmail)
        {
            return _users.FirstOrDefault(u => u.UserEmail == uEmail)!;
        }

        public void AddUser(User user)
        {
            if (user != null)
            {
                if (user.Id == 0)
                {
                    int maxId = 0;
                    int i = 0;
                    while (i < _users.Count)
                    {
                        if (_users[i].Id > maxId) maxId = _users[i].Id;
                        i = i + 1;
                    }
                    user.Id = maxId + 1;
                }

                _users.Add(user);
            }
        }

        public void RemoveUser(User user)
        {
            if (user == null) return;

            int i = 0;
            while (i < _users.Count)
            {
                if (_users[i].Id == user.Id)
                {
                    _users.RemoveAt(i);
                    return;
                }
                i = i + 1;
            }
        }

        public void UpdateUser(User user)
        {
            if (user == null) return;

            int i = 0;
            while (i < _users.Count)
            {
                if (_users[i].Id == user.Id)
                {
                    _users[i].FirstName = user.FirstName;
                    _users[i].LastName = user.LastName;
                    _users[i].UserEmail = user.UserEmail;
                    _users[i].UMobile = user.UMobile;
                    return;
                }
                i = i + 1;
            }
        }
    }
}