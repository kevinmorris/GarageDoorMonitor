namespace GarageDoorApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = (MainPageViewModel)BindingContext;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            _viewModel.FetchStatuses();
        }
    }
}