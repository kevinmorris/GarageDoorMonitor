using System.Reflection;
using GarageDoorApp.Api;
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
            services.AddSingleton<IConfiguration>(config);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}