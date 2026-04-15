namespace WorkoutManagerNoam.Models
{
    public class Challenge
    {
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public double MaxAmount { get; set; }

        public string Difficulty { get; set; } = "";   // Easy / Medium / Hard
        public double RewardAmount { get; set; }       // 10 / 20 / 30

        public bool IsCompleted { get; set; }
        public bool IsClaimed { get; set; }
    }
}