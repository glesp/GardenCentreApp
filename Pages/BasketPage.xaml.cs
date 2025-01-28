using GardenCentreApp.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // For Preferences

namespace GardenCentreApp.Pages
{
    public partial class BasketPage : ContentPage
    {
        public BasketPage()
        {
            InitializeComponent();

            // Default ViewModel for Shell instantiation
            BindingContext = new BasketPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Retrieve the userId from Preferences
            var userId = Preferences.Get("UserId", 0); // Default to 0 if not set

            if (userId != 0) // Check if a valid userId exists
            {
                // Update the ViewModel with the userId
                BindingContext = new BasketPageViewModel(userId);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No userId found in Preferences. Default ViewModel used.");
            }
        }
    }
}