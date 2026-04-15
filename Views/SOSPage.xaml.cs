using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class SOSPage : ContentPage
{
    public SOSPage()
    {
        InitializeComponent();
    }

    private async void TakeSOSMoney_Clicked(object sender, EventArgs e)
    {
        if (!double.TryParse(SOSAmountEntry.Text, out double amount))
        {
            await DisplayAlert("Error", "Enter valid amount", "OK");
            return;
        }

        if (amount <= 0 || amount > 200)
        {
            await DisplayAlert("Error", "SOS amount must be up to 200", "OK");
            return;
        }

        if (AppState.CurrentUser == null)
        {
            await DisplayAlert("Error", "No user found", "OK");
            return;
        }

        AppState.CurrentUser.Balance += amount;

        AppState.Expenses.Add(new ChildExpense
        {
            Category = "SOS",
            Amount = amount,
            Date = DateTime.Now,
            IsSOS = true
        });

        await DisplayAlert("Success", $"¤{amount} added by SOS", "OK");

        SOSAmountEntry.Text = "";
    }
}