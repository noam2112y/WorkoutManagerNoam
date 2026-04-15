namespace WorkoutManagerNoam.Service
{
    public static class FirebaseConfig
    {
        public const string ApiKey = "AIzaSyCoGpMM6OBkqJ0OsOmXogD7_KHK-c3ECsU";
        public const string ProjectId = "kidssavingapp";

        public static string SignUpUrl =>
            $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}";

        public static string SignInUrl =>
            $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}";

        public static string FirestoreBaseUrl =>
            $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents";
    }
}