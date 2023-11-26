using System.Reflection;
using Android.Content;
using GarageDoorApp.Api;
using GarageDoorMonitor;
using Microsoft.Extensions.Configuration;

namespace GarageDoorApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Services = ConfigureServices();

            MainPage = new AppShell();
        }

        public IServiceProvider Services { get; private set; }
        public new static App Current => (App)Application.Current;

        private static IServiceProvider ConfigureServices()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("GarageDoorApp.appsettings.json");
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IGarageDoorApi, GarageDoorApi>();

#if ANDROID
            services.AddSingleton(Microsoft.Maui.ApplicationModel.Platform.AppContext);
            services.AddSingleton<NotificationRegistrar>();

#endif
            services.AddSingleton<IConfiguration>(config);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Created += (sender, args) =>
            {
#if ANDROID
                var notifierRegistrationId = Preferences.Default.Get<string>(Constants.KeyGarageDoorNotifierRegistrationId, null);
                if (notifierRegistrationId == null)
                {
                    var firebaseRegistrar = Services.GetService<NotificationRegistrar>();
                    firebaseRegistrar.RegisteredEvent += FirebaseRegistrar_Registered;
                    firebaseRegistrar.Register();
                }
#endif
            };

            return window;
        }

        private void FirebaseRegistrar_Registered(object sender, NotificationRegisteredEventArgs e)
        {
            Preferences.Default.Set(Constants.KeyGarageDoorNotifierRegistrationId, e.RegistrationId);
        }


    }
}