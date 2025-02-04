using Microsoft.Maui.Controls;
using GardenCentreApp.ViewModels;

namespace GardenCentreApp.Pages
{
    public partial class ProductPage : ContentPage
    {
        private ProductPageViewModel _viewModel;

        public ProductPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as ProductPageViewModel;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            // Get the route from Shell navigation
            string route = Shell.Current.CurrentState.Location.OriginalString;

            // Update the selected category in the ViewModel
            _viewModel?.UpdateCategoryFromRoute(route);
        }
    }
}