using WorkoutManagerNoam.ViewModels;
using WorkoutManagerNoam.Views;

namespace WorkoutManagerNoam
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

            Routing.RegisterRoute(nameof(UserDetailsPage), typeof(UserDetailsPage));
            Routing.RegisterRoute(nameof(UsersListPage), typeof(UsersListPage));
        }
    }
}
