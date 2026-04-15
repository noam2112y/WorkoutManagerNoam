using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

[QueryProperty(nameof(DateValue), "date")]
public partial class DailySummaryPage : ContentPage
{
    private string _dateValue = "";

    public string DateValue
    {
        get => _dateValue;
        set
        {
            _dateValue = value;
            LoadSummary();
        }
    }

    public DailySummaryPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSummary();
    }

    private void LoadSummary()
    {
        if (DateValue == "")
            return;

        DateTime date;
        if (!DateTime.TryParse(DateValue, out date))
            return;

        double food = AppState.GetSpentByCategoryForDate("Food", date);
        double entertainment = AppState.GetSpentByCategoryForDate("Entertainment", date);
        double transport = AppState.GetSpentByCategoryForDate("Transport", date);
        double shopping = AppState.GetSpentByCategoryForDate("Shopping", date);
        double sos = AppState.GetSOSForDate(date);
        double rewards = AppState.GetRewardClaimsForDate(date);

        DateLabel.Text = $"Summary for {date:dd/MM/yyyy}";
        FoodLabel.Text = $"Food: ¤{food}";
        EntertainmentLabel.Text = $"Entertainment: ¤{entertainment}";
        TransportLabel.Text = $"Transport: ¤{transport}";
        ShoppingLabel.Text = $"Shopping: ¤{shopping}";
        SOSLabel.Text = $"SOS: ¤{sos}";
        RewardsLabel.Text = $"Rewards Claimed: ¤{rewards}";

        SetCategoryColor(FoodLabel, "Food", food);
        SetCategoryColor(EntertainmentLabel, "Entertainment", entertainment);
        SetCategoryColor(TransportLabel, "Transport", transport);

        ShoppingLabel.TextColor = Colors.Black;
        SOSLabel.TextColor = Colors.DarkOrange;
        RewardsLabel.TextColor = Colors.Purple;

        bool passedAll = true;

        if (!IsWithinChallenge("Food", food))
            passedAll = false;

        if (!IsWithinChallenge("Entertainment", entertainment))
            passedAll = false;

        if (!IsWithinChallenge("Transport", transport))
            passedAll = false;

        if (passedAll)
        {
            ResultLabel.Text = "Great job - all daily challenges passed";
            ResultLabel.TextColor = Colors.Green;
        }
        else
        {
            ResultLabel.Text = "Needs improvement - some limits were passed";
            ResultLabel.TextColor = Colors.Red;
        }
    }

    private void SetCategoryColor(Label label, string category, double amount)
    {
        if (IsWithinChallenge(category, amount))
            label.TextColor = Colors.Green;
        else
            label.TextColor = Colors.Red;
    }

    private bool IsWithinChallenge(string category, double amount)
    {
        for (int i = 0; i < AppState.Challenges.Count; i++)
        {
            if (AppState.Challenges[i].Category == category)
                return amount <= AppState.Challenges[i].MaxAmount;
        }

        return true;
    }

    private async void ViewExpenses_Clicked(object sender, EventArgs e)
    {
        if (DateValue == "")
            return;

        await Shell.Current.GoToAsync($"{nameof(ParentExpensesPage)}?date={DateValue}");
    }
}