using WorkoutManagerNoam.ViewModels;
using WorkoutManagerNoam.Views;

namespace WorkoutManagerNoam
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

            Routing.RegisterRoute(nameof(ParentHomePage), typeof(ParentHomePage));
            Routing.RegisterRoute(nameof(ChildHomePage), typeof(ChildHomePage));
            Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
            Routing.RegisterRoute(nameof(ChallengesPage), typeof(ChallengesPage));
            Routing.RegisterRoute(nameof(DailyRecommendationsPage), typeof(DailyRecommendationsPage));
            Routing.RegisterRoute(nameof(ChildExpensePage), typeof(ChildExpensePage));
            Routing.RegisterRoute(nameof(AddChallengePage), typeof(AddChallengePage));
            Routing.RegisterRoute(nameof(ChallengesPage), typeof(ChallengesPage));
            Routing.RegisterRoute(nameof(SOSPage), typeof(SOSPage));
            Routing.RegisterRoute(nameof(ParentExpensesPage), typeof(ParentExpensesPage));
            Routing.RegisterRoute(nameof(ChildExpensePage), typeof(ChildExpensePage));
            Routing.RegisterRoute(nameof(ExpenseDaysPage), typeof(ExpenseDaysPage));
            Routing.RegisterRoute(nameof(DailySummaryPage), typeof(DailySummaryPage));

        }
    }
}