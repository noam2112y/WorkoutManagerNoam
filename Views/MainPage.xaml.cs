using WorkoutManagerNoam.ViewModels;

namespace WorkoutManagerNoam.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.Navigation = Navigation;
    }
}