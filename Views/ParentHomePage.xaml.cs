using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class ParentHomePage : ContentPage
{
    private readonly IDBService _db;
    private List<User> _children = new List<User>();

    public ParentHomePage(IDBService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadChildren();
    }

    private void LoadChildren()
    {
        _children.Clear();
        ChildrenPicker.Items.Clear();

        List<User> allUsers = _db.GetAllUsers();

        for (int i = 0; i < allUsers.Count; i++)
        {
            if (!allUsers[i].IsParent)
            {
                _children.Add(allUsers[i]);
                ChildrenPicker.Items.Add(allUsers[i].FirstName);
            }
        }

        if (_children.Count == 0)
        {
            ChildLabel.Text = "No child found";
            AppState.SelectedChild = null;
        }
    }

    private void ChildrenPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = ChildrenPicker.SelectedIndex;

        if (index < 0 || index >= _children.Count)
            return;

        AppState.SelectedChild = _children[index];
        ChildLabel.Text = $"Selected child: {_children[index].FirstName}";
    }
    private async void ViewExpenses_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ParentExpensesPage));
    }

    private async void AddChallenge_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddChallengePage));
    }
    private async void ExpenseDays_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ExpenseDaysPage));
    }
    private void Add50_Clicked(object sender, EventArgs e)
    {
        AppState.PendingAmount = 50;
        AmountLabel.Text = "Selected amount: 50";
    }

    private void Add100_Clicked(object sender, EventArgs e)
    {
        AppState.PendingAmount = 100;
        AmountLabel.Text = "Selected amount: 100";
    }

    private void Add200_Clicked(object sender, EventArgs e)
    {
        AppState.PendingAmount = 200;
        AmountLabel.Text = "Selected amount: 200";
    }

    private async void ContinueToPayment_Clicked(object sender, EventArgs e)
    {
        if (AppState.SelectedChild == null)
        {
            await DisplayAlert("Error", "Please choose a child first", "OK");
            return;
        }

        if (AppState.PendingAmount <= 0)
        {
            await DisplayAlert("Error", "Please select an amount first", "OK");
            return;
        }

        await Shell.Current.GoToAsync(nameof(PaymentPage));
    }
}