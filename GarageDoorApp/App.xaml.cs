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
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IGarageDoorApi, GarageDoorApi>();
            services.AddSingleton<IConfiguration>(config);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}