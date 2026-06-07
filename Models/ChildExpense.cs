namespace WorkoutManagerNoam.Models
{
    // מייצג הוצאה שבוצעה על ידי ילד.
    public class ChildExpense
    {
        public string Category { get; set; } = "";

        public double Amount { get; set; }

        // תאריך ביצוע ההוצאה.
        public DateTime Date { get; set; }

        public bool IsSOS { get; set; }
    }
}