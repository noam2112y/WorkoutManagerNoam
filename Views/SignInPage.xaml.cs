using WorkoutManagerNoam.ViewModels;

namespace WorkoutManagerNoam.Views;

public partial class SignInPage : ContentPage
{
    public SignInPage(SignInViewModel vm)
    {
        InitializeComponent();
        vm.Navigation = this.Navigation;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SignInViewModel vm)
            vm.Navigation = this.Navigation;
    }
   
}
