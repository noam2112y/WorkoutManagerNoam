using WorkoutManagerNoam.ViewModels;

namespace WorkoutManagerNoam.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpViewModel vm)
    {
        InitializeComponent();

        vm.Navigation = this.Navigation;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SignUpViewModel vm)
            vm.Navigation = this.Navigation;
    }
}
