using WorkoutManagerNoam.Service;

namespace WorkoutManagerNoam.Views;

public partial class PaymentPage : ContentPage
{
    private readonly IDBService _db;

    public PaymentPage(IDBService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        AmountLabel.Text = $"Amount: {AppState.PendingAmount}";
    }

    private async void PayNow_Clicked(object sender, EventArgs e)
    {
        string holder = CardHolderEntry.Text ?? "";
        string card = CardNumberEntry.Text ?? "";
        string expiry = ExpiryEntry.Text ?? "";
        string cvv = CvvEntry.Text ?? "";

        if (holder.Trim() == "" ||
            card.Trim() == "" ||
            expiry.Trim() == "" ||
            cvv.Trim() == "")
        {
            await DisplayAlert("Error", "Please fill all payment details", "OK");
            return;
        }

        if (AppState.SelectedChild == null)
        {
            await DisplayAlert("Error", "No child selected", "OK");
            return;
        }

        AppState.SelectedChild.Balance += AppState.PendingAmount;

        _db.UpdateUser(AppState.SelectedChild);

        await DisplayAlert(
            "Success",
            $"Payment completed.\n{AppState.SelectedChild.FirstName} new balance: {AppState.SelectedChild.Balance}",
            "OK");

        AppState.PendingAmount = 0;
        AppState.SelectedChild = null;

        await Shell.Current.GoToAsync(nameof(ParentHomePage));
    }
}