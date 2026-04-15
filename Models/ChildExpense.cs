namespace WorkoutManagerNoam.Models
{
    public class ChildExpense
    {
        public string Category { get; set; } = "";
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public bool IsSOS { get; set; }
    }
}