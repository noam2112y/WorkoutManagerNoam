using WorkoutManagerNoam.Models;

namespace WorkoutManagerNoam.Service
{
    public static class AppState
    {
        public static User? CurrentUser { get; set; }
        public static User? SelectedUser { get; set; }
        public static User? SelectedChild { get; set; }

        public static bool IsParentLoggedIn =>
            CurrentUser != null && CurrentUser.IsParent;

        public static double PendingAmount { get; set; }

        public static bool DayFinished { get; set; }

        public static List<ChildExpense> Expenses { get; set; } = new List<ChildExpense>();
        public static List<RewardClaim> RewardClaims { get; set; } = new List<RewardClaim>();

        public static List<Challenge> Challenges { get; set; } = new List<Challenge>
        {
            new Challenge
            {
                Title = "Food limit 50 per day",
                Category = "Food",
                MaxAmount = 50,
                Difficulty = "Easy",
                RewardAmount = 10,
                IsCompleted = false,
                IsClaimed = false
            },
            new Challenge
            {
                Title = "Entertainment limit 80 per day",
                Category = "Entertainment",
                MaxAmount = 80,
                Difficulty = "Medium",
                RewardAmount = 20,
                IsCompleted = false,
                IsClaimed = false
            },
            new Challenge
            {
                Title = "Transport limit 40 per day",
                Category = "Transport",
                MaxAmount = 40,
                Difficulty = "Hard",
                RewardAmount = 30,
                IsCompleted = false,
                IsClaimed = false
            }
        };

        public static List<string> Recommendations { get; set; } = new List<string>
        {
            "Save 10 today instead of spending",
            "Bring food from home instead of buying",
            "Avoid buying snacks today",
            "Walk instead of taking a ride",
            "Set a savings goal for this week",
            "Spend only on things you really need"
        };

        public static void CheckChallengesForToday()
        {
            for (int i = 0; i < Challenges.Count; i++)
            {
                double total = 0;

                for (int j = 0; j < Expenses.Count; j++)
                {
                    if (Expenses[j].Date.Date == DateTime.Now.Date &&
                        Expenses[j].Category == Challenges[i].Category &&
                        !Expenses[j].IsSOS)
                    {
                        total += Expenses[j].Amount;
                    }
                }

                Challenges[i].IsCompleted = total <= Challenges[i].MaxAmount;
            }
        }

        public static void StartNewDay()
        {
            DayFinished = false;

            for (int i = 0; i < Challenges.Count; i++)
            {
                Challenges[i].IsCompleted = false;
                Challenges[i].IsClaimed = false;
            }
        }

        public static double GetTodayTotalSpent()
        {
            double total = 0;

            for (int i = 0; i < Expenses.Count; i++)
            {
                if (Expenses[i].Date.Date == DateTime.Now.Date && !Expenses[i].IsSOS)
                    total += Expenses[i].Amount;
            }

            return total;
        }

        public static double GetTodayTotalSOS()
        {
            double total = 0;

            for (int i = 0; i < Expenses.Count; i++)
            {
                if (Expenses[i].Date.Date == DateTime.Now.Date && Expenses[i].IsSOS)
                    total += Expenses[i].Amount;
            }

            return total;
        }

        public static List<DateTime> GetExpenseDays()
        {
            List<DateTime> days = new List<DateTime>();

            for (int i = 0; i < Expenses.Count; i++)
            {
                DateTime day = Expenses[i].Date.Date;
                bool exists = false;

                for (int j = 0; j < days.Count; j++)
                {
                    if (days[j] == day)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    days.Add(day);
            }

            for (int i = 0; i < RewardClaims.Count; i++)
            {
                DateTime day = RewardClaims[i].Date.Date;
                bool exists = false;

                for (int j = 0; j < days.Count; j++)
                {
                    if (days[j] == day)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    days.Add(day);
            }

            days.Sort();
            days.Reverse();

            return days;
        }

        public static double GetSpentByCategoryForDate(string category, DateTime date)
        {
            double total = 0;

            for (int i = 0; i < Expenses.Count; i++)
            {
                if (Expenses[i].Date.Date == date.Date &&
                    Expenses[i].Category == category &&
                    !Expenses[i].IsSOS)
                {
                    total += Expenses[i].Amount;
                }
            }

            return total;
        }

        public static double GetSOSForDate(DateTime date)
        {
            double total = 0;

            for (int i = 0; i < Expenses.Count; i++)
            {
                if (Expenses[i].Date.Date == date.Date && Expenses[i].IsSOS)
                    total += Expenses[i].Amount;
            }

            return total;
        }

        public static double GetRewardClaimsForDate(DateTime date)
        {
            double total = 0;

            for (int i = 0; i < RewardClaims.Count; i++)
            {
                if (RewardClaims[i].Date.Date == date.Date)
                    total += RewardClaims[i].Amount;
            }

            return total;
        }
    }
}