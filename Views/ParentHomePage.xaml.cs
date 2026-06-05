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
        AppState.SelectedChild = null;

        User? currentParent = AppState.CurrentUser;

        if (currentParent == null || !currentParent.IsParent)
        {
            ChildLabel.Text = "Only parent users can choose a child";
            return;
        }

        List<User> allUsers = _db.GetAllUsers();

        for (int i = 0; i < allUsers.Count; i++)
        {
            if (!allUsers[i].IsParent && allUsers[i].ParentId == currentParent.Id)
            {
                _children.Add(allUsers[i]);
                ChildrenPicker.Items.Add(allUsers[i].FirstName + " " + allUsers[i].LastName);
            }
        }

        if (_children.Count == 0)
        {
            ChildLabel.Text = "No child connected to this parent";
        }
        else
        {
            ChildLabel.Text = "No child selected";
        }
    }

    private async void ChildrenPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = ChildrenPicker.SelectedIndex;

        if (index < 0 || index >= _children.Count)
        {
            AppState.SelectedChild = null;
            return;
        }

        User? currentParent = AppState.CurrentUser;

        if (currentParent == null || !currentParent.IsParent)
        {
            AppState.SelectedChild = null;
            await DisplayAlert("Error", "Only parent users can choose a child", "OK");
            return;
        }

        User selectedChild = _children[index];

        if (selectedChild.ParentId != currentParent.Id)
        {
            AppState.SelectedChild = null;
            await DisplayAlert("Error", "This child is not connected to your account", "OK");
            return;
        }

        AppState.SelectedChild = selectedChild;
        ChildLabel.Text = "Selected child: " + selectedChild.FirstName;
    }

    private async void AddChild_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignUpPage));
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

        User? currentParent = AppState.CurrentUser;

        if (currentParent == null || !currentParent.IsParent)
        {
            await DisplayAlert("Error", "Only parent users can continue to payment", "OK");
            return;
        }

        if (AppState.SelectedChild.ParentId != currentParent.Id)
        {
            AppState.SelectedChild = null;
            await DisplayAlert("Error", "This child is not connected to your account", "OK");
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