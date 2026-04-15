using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class AddChallengePage : ContentPage
{
    public AddChallengePage()
    {
        InitializeComponent();
    }

    private async void SaveChallenge_Clicked(object sender, EventArgs e)
    {
        string title = TitleEntry.Text ?? "";

        if (title.Trim() == "")
        {
            await DisplayAlert("Error", "Enter title", "OK");
            return;
        }

        if (CategoryPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Choose category", "OK");
            return;
        }

        if (!double.TryParse(MaxAmountEntry.Text, out double maxAmount))
        {
            await DisplayAlert("Error", "Enter valid max amount", "OK");
            return;
        }

        AppState.Challenges.Add(new Challenge
        {
            Title = title,
            Category = CategoryPicker.SelectedItem.ToString(),
            MaxAmount = maxAmount
        });

        await DisplayAlert("Success", "Challenge added", "OK");

        await Shell.Current.GoToAsync("..");
    }
}