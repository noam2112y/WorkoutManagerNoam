using WorkoutManagerNoam.Views;

namespace WorkoutManagerNoam;

public partial class App : Application
{
    public Models.User? CurrentUser { get; set; }

    private readonly SignInPage _signInPage;

    public App(SignInPage signInPage)
    {
        InitializeComponent();
        _signInPage = signInPage;
    }

    // ✅ ב-Windows/NET9 זה יותר יציב מלהגדיר MainPage ישירות
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(_signInPage));
    }
}
