using System.Net.Http;
using System.Text;
using System.Text.Json;
using WorkoutManagerNoam.Models;

namespace WorkoutManagerNoam.Service
{
    public class FirebaseDBService : IDBService
    {
        private const string FirebaseUrl =
            "https://workoutmanagernoam-260fb-default-rtdb.europe-west1.firebasedatabase.app/";

        private readonly HttpClient _httpClient;

        public FirebaseDBService()
        {
            _httpClient = new HttpClient();
        }

        public List<User> GetAllUsers()
        {
            string url = FirebaseUrl + "users.json";

            string json = _httpClient.GetStringAsync(url).Result;

            if (string.IsNullOrWhiteSpace(json) || json == "null")
                return new List<User>();

            try
            {
                Dictionary<string, User>? data =
                    JsonSerializer.Deserialize<Dictionary<string, User>>(json);

                if (data == null)
                    return new List<User>();

                return data.Values.ToList();
            }
            catch
            {
                try
                {
                    List<User>? list =
                        JsonSerializer.Deserialize<List<User>>(json);

                    if (list == null)
                        return new List<User>();

                    List<User> cleanList = new List<User>();

                    int i = 0;

                    while (i < list.Count)
                    {
                        if (list[i] != null)
                            cleanList.Add(list[i]);

                        i++;
                    }

                    return cleanList;
                }
                catch
                {
                    return new List<User>();
                }
            }
        }

        public User GetUserByEmail(string uEmail)
        {
            List<User> users = GetAllUsers();

            int i = 0;

            while (i < users.Count)
            {
                if (users[i].UserEmail == uEmail)
                    return users[i];

                i++;
            }

            return null!;
        }

        public bool IsExist(string uEmail, string uPass)
        {
            List<User> users = GetAllUsers();

            int i = 0;

            while (i < users.Count)
            {
                if (users[i].UserEmail == uEmail &&
                    users[i].UserPassword == uPass)
                {
                    return true;
                }

                i++;
            }

            return false;
        }

        public void AddUser(User user)
        {
            List<User> users = GetAllUsers();

            int maxId = 0;

            int i = 0;

            while (i < users.Count)
            {
                if (users[i].Id > maxId)
                    maxId = users[i].Id;

                i++;
            }

            user.Id = maxId + 1;

            string url = FirebaseUrl + "users/" + user.Id + ".json";

            string json = JsonSerializer.Serialize(user);

            StringContent content =
                new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.PutAsync(url, content).Wait();
        }

        public void RemoveUser(User user)
        {
            string url = FirebaseUrl + "users/" + user.Id + ".json";

            _httpClient.DeleteAsync(url).Wait();
        }

        public void UpdateUser(User user)
        {
            string url = FirebaseUrl + "users/" + user.Id + ".json";

            string json = JsonSerializer.Serialize(user);

            StringContent content =
                new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.PutAsync(url, content).Wait();
        }
    }
}