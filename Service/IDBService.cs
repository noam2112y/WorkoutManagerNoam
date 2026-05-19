using WorkoutManagerNoam.Models;

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
}