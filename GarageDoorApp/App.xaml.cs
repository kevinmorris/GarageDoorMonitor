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
            var services = new ServiceCollection();


            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}