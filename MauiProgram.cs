using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using WorkoutManagerNoam.Service;
using WorkoutManagerNoam.ViewModels;
using WorkoutManagerNoam.Views;

namespace WorkoutManagerNoam
{
    public static class MauiProgram
    {
        // ✅ במקום Current – נשמור IServiceProvider אמיתי
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

            // ✅ שומרים גישה לשירותים
            Services = app.Services;

            return app;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            // Pages
            builder.Services.AddSingleton<SignInPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<UsersListPage>();

            // אם יש לך UserDetailsPage – כדאי לרשום גם אותו:
            builder.Services.AddTransient<UserDetailsPage>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            // ViewModels
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddTransient<UsersListPageViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            // Services
            builder.Services.AddSingleton<IDBService, DBMokup>();
            builder.Services.AddTransient<AppShell>();

            return builder;
        }
    }
}
