namespace WorkoutManagerNoam.Models
{
    // מייצג משתמש במערכת: הורה או ילד.
    public class User
    {
        // כתובת האימייל של המשתמש, משמשת לזיהוי ולהתחברות.
        public string? UserEmail { get; set; }

        // סיסמת המשתמש.
        public string? UserPassword { get; set; }

        // פרטים אישיים של המשתמש.
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // מספר טלפון של המשתמש.
        public string? UMobile { get; set; }

        // קובע את סוג המשתמש:
        // true = הורה, false = ילד.
        public bool IsParent { get; set; }

        // יתרת הכסף של המשתמש.
        public double Balance { get; set; }

        // מזהה ייחודי של המשתמש במערכת.
        public int Id { get; set; }

        public DateTime UBDate { get; set; }

        public DateTime RegDate { get; set; }

        // מאפיין נוסף לאימייל, לשימוש במקומות שבהם הקוד עובד עם השם UEmail.
        public string? UEmail
        {
            get { return UserEmail; }
            set { UserEmail = value; }
        }

        // מזהה ההורה שאליו הילד משויך.
        // אצל משתמש מסוג הורה הערך יכול להישאר 0.
        public int ParentId { get; set; }
    }
}