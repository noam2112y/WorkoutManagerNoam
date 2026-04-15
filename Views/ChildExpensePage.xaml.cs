using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class ChildExpensePage : ContentPage
{
    public ChildExpensePage()
    {
        InitializeComponent();
    }

    private async void Pay_Clicked(object sender, EventArgs e)
    {
        if (AppState.DayFinished)
        {
            await DisplayAlert("Blocked", "Day already finished", "OK");
            return;
        }

        if (CategoryPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Choose category", "OK");
            return;
        }

        if (!double.TryParse(AmountEntry.Text, out double amount))
        {
            await DisplayAlert("Error", "Enter valid amount", "OK");
            return;
        }

        string category = CategoryPicker.SelectedItem.ToString() ?? "";

        double todaySpent = 0;

        for (int i = 0; i < AppState.Expenses.Count; i++)
        {
            if (AppState.Expenses[i].Category == category &&
                AppState.Expenses[i].Date.Date == DateTime.Now.Date &&
                !AppState.Expenses[i].IsSOS)
            {
                todaySpent += AppState.Expenses[i].Amount;
            }
        }

        Challenge? matchingChallenge = null;

        for (int i = 0; i < AppState.Challenges.Count; i++)
        {
            if (AppState.Challenges[i].Category == category)
            {
                matchingChallenge = AppState.Challenges[i];
                break;
            }
        }

        if (matchingChallenge != null && todaySpent + amount > matchingChallenge.MaxAmount)
        {
            await DisplayAlert("Blocked",
                $"You cannot pay more than {matchingChallenge.MaxAmount} for {category} today",
                "OK");
            return;
        }

        if (AppState.CurrentUser == null)
        {
            await DisplayAlert("Error", "No user found", "OK");
            return;
        }

        if (AppState.CurrentUser.Balance < amount)
        {
            await DisplayAlert("Error", "Not enough balance", "OK");
            return;
        }

        AppState.CurrentUser.Balance -= amount;

        AppState.Expenses.Add(new ChildExpense
        {
            Category = category,
            Amount = amount,
            Date = DateTime.Now,
            IsSOS = false
        });

        await DisplayAlert("Success", "Payment completed", "OK");

        AmountEntry.Text = "";
        CategoryPicker.SelectedIndex = -1;
    }
}