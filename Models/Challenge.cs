namespace WorkoutManagerNoam.Models
{
    // מייצג אתגר שההורה מגדיר לילד.
    public class Challenge
    {
        public string Title { get; set; } = "";

   
        public string Category { get; set; } = "";

        // הסכום המקסימלי שמותר להוציא במסגרת האתגר.
        public double MaxAmount { get; set; }

        // רמת הקושי של האתגר.
        public string Difficulty { get; set; } = "";

        // סכום התגמול שהילד יקבל לאחר השלמת האתגר.
        public double RewardAmount { get; set; }

        public bool IsCompleted { get; set; }

        // מסמן האם התגמול כבר נלקח, כדי למנוע קבלת תגמול כפול.
        public bool IsClaimed { get; set; }
    }
}