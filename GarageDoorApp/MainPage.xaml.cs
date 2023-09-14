namespace GarageDoorApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = (MainPageViewModel)BindingContext;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            _viewModel.Action();
        }
    }
}