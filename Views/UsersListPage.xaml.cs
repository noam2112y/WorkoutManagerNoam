using WorkoutManagerNoam.ViewModels;

namespace WorkoutManagerNoam.Views
{
    public partial class UsersListPage : ContentPage
    {
        public UsersListPage(UsersListPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as UsersListPageViewModel)?.GetAllUsersCommand.Execute(null);
        }
    }
}
