using WorkoutManagerNoam.Models;

namespace WorkoutManagerNoam
{
    public static class AppState
    {
        // האם אדמין מחובר
        public static bool IsAdminLoggedIn { get; set; }

        // משתמש אדמין (אם אתה רוצה לשמור אותו)
        public static User? AdminUser { get; set; }

        // אם אתה עדיין משתמש בזה במקום אחר:
        public static ObservableUser? SelectedUser { get; set; }
    }
}
