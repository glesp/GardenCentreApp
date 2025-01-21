using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GardenCentreApp.Models;

namespace GardenCentreApp.ViewModels
{
    public partial class BasketPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BasketItem> basketItems;

        public double TotalPrice => BasketItems.Sum(item => (item.Product?.Price ?? 0) * item.Quantity);

        // Default constructor for XAML
        public BasketPageViewModel()
        {
            BasketItems = new ObservableCollection<BasketItem>();
        }

        // Constructor with userId parameter
        public BasketPageViewModel(int userId)
        {
            BasketItems = new ObservableCollection<BasketItem>(App.Database.GetBasketItemsWithProducts(userId));
        }
        
        // Command for removing an item from the basket
        [RelayCommand]
        private void RemoveItem(BasketItem item)
        {
            if (item == null) return;

            // Remove the item from the database
            App.Database.RemoveBasketItem(item.Id);

            // Remove the item from the ObservableCollection
            BasketItems.Remove(item);

            // Notify the UI that the total price has changed
            OnPropertyChanged(nameof(TotalPrice));
        }
        
        [RelayCommand]
        private async void Checkout()
        {
            // Clear the basket in the database
            var userId = Preferences.Get("UserId", 0); // Get the logged-in user ID
            App.Database.ClearBasket(userId);

            // Clear the local ObservableCollection
            BasketItems.Clear();

            // Notify the UI to update the total price
            OnPropertyChanged(nameof(TotalPrice));

            // Display a success message
            await Application.Current.MainPage.DisplayAlert("Success", "Your purchase has been completed!", "OK");

            // Optionally navigate to another page, e.g., ProductPage
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}