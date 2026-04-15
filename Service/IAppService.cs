using WorkoutManagerNoam.Models;

namespace WorkoutManagerNoam.Service
{
    public interface IAppService
    {
        void AddUser(User user);
        User? SignIn(string email, string password);
        bool IsEmailExists(string email);
        List<User> GetAllUsers();
        void AddBalance(double amount);
    }
}