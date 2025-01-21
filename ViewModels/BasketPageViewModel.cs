using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GardenCentreApp.Models;
using GardenCentreApp.Pages;

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
            var userId = Preferences.Get("UserId", 0);
            var isCorporateClient = Preferences.Get("IsCorporateClient", false);

            System.Diagnostics.Debug.WriteLine($"IsCorporateClient during checkout: {isCorporateClient}");

            try
            {
                if (isCorporateClient)
                {
                    // Navigate to Monthly Purchases Page
                    await Application.Current.MainPage.Navigation.PushAsync(new MonthlyPurchasesPage());
                }
                else
                {
                    // Validate basket before proceeding
                    if (BasketItems == null || !BasketItems.Any())
                    {
                        System.Diagnostics.Debug.WriteLine("Basket is empty during checkout.");
                        return;
                    }

                    // Copy items before clearing
                    var purchasedItems = new ObservableCollection<BasketItem>(BasketItems);

                    // Clear the basket
                    App.Database.ClearBasket(userId);
                    BasketItems.Clear();
                    OnPropertyChanged(nameof(TotalPrice));

                    // Navigate to Thank You Page with purchased items
                    await Application.Current.MainPage.Navigation.PushAsync(new ThankYouPage(purchasedItems));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }




    }
}