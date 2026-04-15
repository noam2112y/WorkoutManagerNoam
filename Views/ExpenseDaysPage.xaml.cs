using WorkoutManagerNoam.Service;
using static System.Collections.Specialized.NameObjectCollectionBase;

namespace WorkoutManagerNoam.Views;

public partial class ExpenseDaysPage : ContentPage
{
    public ExpenseDaysPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        DaysCollection.ItemsSource = null;
        DaysCollection.ItemsSource = AppState.GetExpenseDays();
    }

    private async void DaysCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        DateTime selectedDate = (DateTime)e.CurrentSelection[0];

        await Shell.Current.GoToAsync($"{nameof(DailySummaryPage)}?date={selectedDate:yyyy-MM-dd}");
    }
}