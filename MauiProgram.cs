using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.ViewModels;
using WorkoutManagerNoam.Views;

namespace WorkoutManagerNoam
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; }

        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

            builder.RegisterViews()
                   .RegisterViewModels()
                   .RegisterServices();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            MauiApp app = builder.Build();
            Services = app.Services;

            return app;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddTransient<SignUpPage>();

            builder.Services.AddTransient<ParentHomePage>();
            builder.Services.AddTransient<ChildHomePage>();
            builder.Services.AddTransient<PaymentPage>();

            builder.Services.AddTransient<ChallengesPage>();
            builder.Services.AddTransient<DailyRecommendationsPage>();
            builder.Services.AddTransient<ChildExpensePage>();
            builder.Services.AddTransient<AddChallengePage>();
            builder.Services.AddTransient<ChallengesPage>();
            builder.Services.AddTransient<SOSPage>();
            builder.Services.AddTransient<ParentExpensesPage>();
            builder.Services.AddTransient<ChildExpensePage>();
            builder.Services.AddTransient<ExpenseDaysPage>();
            builder.Services.AddTransient<DailySummaryPage>();

            builder.Services.AddTransient<AppShell>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();

            builder.Services.AddSingleton<AppShellViewModel>();

            //builder.Services.AddTransient<UsersListPageViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDBService, DBMokup>();

            return builder;
        }
    }
}