using WorkoutManagerNoam.Views;

namespace WorkoutManagerNoam;

public partial class App : Application
{
    public Models.User? CurrentUser { get; set; }

    private readonly MainPage _mainPage;

    public App(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(_mainPage));
    }
}