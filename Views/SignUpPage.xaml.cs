using WorkoutManagerNoam.ViewModels;

namespace WorkoutManagerNoam.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.Navigation = Navigation;
    }
}