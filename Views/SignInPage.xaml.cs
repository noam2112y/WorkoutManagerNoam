using WorkoutManagerNoam.ViewModels;

namespace WorkoutManagerNoam.Views;

public partial class SignInPage : ContentPage
{
    public SignInPage(SignInViewModel vm)
    {
        InitializeComponent();

        vm.Navigation = Navigation;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SignInViewModel vm)
        {
            vm.Navigation = Navigation;
            await vm.CheckRememberedUserAsync();
        }
    }
}