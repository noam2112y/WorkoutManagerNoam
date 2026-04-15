using WorkoutManagerNoam.Views;
using System.Windows.Input;

namespace WorkoutManagerNoam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IServiceProvider _sp;

        public INavigation Navigation { get; set; }

        public ICommand GoToSignInCommand { get; }
        public ICommand GoToSignUpCommand { get; }

        public MainPageViewModel(IServiceProvider serviceProvider)
        {
            _sp = serviceProvider;

            GoToSignInCommand = new Command(async () => await GoToSignIn());
            GoToSignUpCommand = new Command(async () => await GoToSignUp());
        }

        private async Task GoToSignIn()
        {
            if (Navigation == null)
                return;

            SignInPage page = _sp.GetService(typeof(SignInPage)) as SignInPage;
            if (page != null)
                await Navigation.PushAsync(page);
        }

        private async Task GoToSignUp()
        {
            if (Navigation == null)
                return;

            SignUpPage page = _sp.GetService(typeof(SignUpPage)) as SignUpPage;
            if (page != null)
                await Navigation.PushAsync(page);
        }
    }
}