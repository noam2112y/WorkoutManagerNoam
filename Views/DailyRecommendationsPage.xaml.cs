using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class DailyRecommendationsPage : ContentPage
{
    private Random _random = new Random();

    public DailyRecommendationsPage()
    {
        InitializeComponent();
    }

    private void GetRecommendation_Clicked(object sender, EventArgs e)
    {
        if (AppState.Recommendations == null || AppState.Recommendations.Count == 0)
        {
            RecommendationLabel.Text = "No recommendations available";
            return;
        }

        int index = _random.Next(AppState.Recommendations.Count);
        RecommendationLabel.Text = AppState.Recommendations[index];
    }
}