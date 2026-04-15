using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class ChildHomePage : ContentPage
{
    public ChildHomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (AppState.CurrentUser != null)
        {
            WelcomeLabel.Text = $"Welcome {AppState.CurrentUser.FirstName}";
            BalanceLabel.Text = $"Balance: {AppState.CurrentUser.Balance}";
            TodaySpentLabel.Text = $"Today spent: ¤{AppState.GetTodayTotalSpent()}";
            TodaySOSLabel.Text = $"Today SOS: ¤{AppState.GetTodayTotalSOS()}";
        }
    }

    private async void AddExpense_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChildExpensePage));
    }

    private async void SOS_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SOSPage));
    }

    private async void FinishDay_Clicked(object sender, EventArgs e)
    {
        AppState.DayFinished = true;
        AppState.CheckChallengesForToday();

        await Shell.Current.GoToAsync(nameof(ChallengesPage));
    }
    private async void StartNewDay_Clicked(object sender, EventArgs e)
    {
        AppState.StartNewDay();
        await DisplayAlert("New Day", "A new day has started", "OK");
    }

    private async void Challenges_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChallengesPage));
    }

    private async void DailyRecommendations_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(DailyRecommendationsPage));
    }
}