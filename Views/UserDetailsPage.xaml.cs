using WorkoutManagerNoam.Models;
using WorkoutManagerNoam.Service;
using Microsoft.Extensions.DependencyInjection;

namespace WorkoutManagerNoam.Views
{
    public partial class UserDetailsPage : ContentPage
    {
        private readonly IDBService _db;
        private ObservableUser? _selected;

        public UserDetailsPage()
        {
            InitializeComponent();
            _db = MauiProgram.Services.GetService<IDBService>();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            _selected = AppState.SelectedUser;   // ✅ זה מה שחסר לך
            if (_selected == null) return;

            FirstNameEntry.Text = _selected.FirstName ?? "";
            LastNameEntry.Text = _selected.LastName ?? "";
            EmailEntry.Text = _selected.UEmail ?? "";
            MobileEntry.Text = _selected.UMobile ?? "";
        }


        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            if (_selected == null) return;

            User updated = new User
            {
                Id = _selected.Id,
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                UserEmail = EmailEntry.Text,   // ✅ DBMokup מעדכן לפי UserEmail
                UMobile = MobileEntry.Text
            };

            _db.UpdateUser(updated);

            _selected.FirstName = updated.FirstName;
            _selected.LastName = updated.LastName;
            _selected.UEmail = updated.UserEmail;
            _selected.UMobile = updated.UMobile;

            await Shell.Current.GoToAsync("..");
        }

    }
}
