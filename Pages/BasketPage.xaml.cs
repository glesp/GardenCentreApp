using Microsoft.Maui.Controls;

namespace GardenCentreApp.Pages
{
    public partial class BasketPage : ContentPage
    {
        public BasketPage(int userId)
        {
            InitializeComponent();
            BindingContext = new ViewModels.BasketPageViewModel(userId);
        }
    }
}