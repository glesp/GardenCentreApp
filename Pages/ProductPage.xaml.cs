using Microsoft.Maui.Controls;
using GardenCentreApp.ViewModels;

namespace GardenCentreApp.Pages
{
    public partial class ProductPage : ContentPage
    {
        public ProductPage()
        {
            InitializeComponent();

            // Set the BindingContext to the ViewModel
            BindingContext = new ProductPageViewModel();
        }
    }
}