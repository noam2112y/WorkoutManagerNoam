using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

[QueryProperty(nameof(DateValue), "date")]
public partial class ParentExpensesPage : ContentPage
{
    private string _dateValue = "";

    public string DateValue
    {
        get => _dateValue;
        set
        {
            _dateValue = value;
            LoadExpenses();
        }
    }

    public ParentExpensesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadExpenses();
    }

    private void LoadExpenses()
    {
        if (ExpensesCollection == null)
            return;

        List<ChildExpense> filtered = new List<ChildExpense>();

        if (DateValue == "")
        {
            ExpensesCollection.ItemsSource = AppState.Expenses;
            return;
        }

        DateTime date;

        if (!DateTime.TryParse(DateValue, out date))
        {
            ExpensesCollection.ItemsSource = AppState.Expenses;
            return;
        }

        for (int i = 0; i < AppState.Expenses.Count; i++)
        {
            if (AppState.Expenses[i].Date.Date == date.Date)
                filtered.Add(AppState.Expenses[i]);
        }

        ExpensesCollection.ItemsSource = null;
        ExpensesCollection.ItemsSource = filtered;
    }
}