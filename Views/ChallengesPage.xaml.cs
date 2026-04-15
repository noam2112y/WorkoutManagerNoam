using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class ChallengesPage : ContentPage
{
    public ChallengesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ChallengesCollection.ItemsSource = null;
        ChallengesCollection.ItemsSource = AppState.Challenges;
    }

    private async void ClaimReward_Clicked(object sender, EventArgs e)
    {
        Button? button = sender as Button;
        Challenge? challenge = button?.BindingContext as Challenge;

        if (challenge == null)
            return;

        if (!challenge.IsCompleted)
            return;

        if (challenge.IsClaimed)
            return;

        if (AppState.CurrentUser == null)
            return;

        AppState.CurrentUser.Balance += challenge.RewardAmount;
        challenge.IsClaimed = true;

        AppState.RewardClaims.Add(new RewardClaim
        {
            ChallengeTitle = challenge.Title,
            Amount = challenge.RewardAmount,
            Date = DateTime.Now
        });

        await DisplayAlert("Success",
            $"¤{challenge.RewardAmount} added to your balance",
            "OK");

        ChallengesCollection.ItemsSource = null;
        ChallengesCollection.ItemsSource = AppState.Challenges;
    }
}