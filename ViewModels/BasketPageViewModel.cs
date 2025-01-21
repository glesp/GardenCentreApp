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
            var userId = Preferences.Get("UserId", 0);
            var isCorporateClient = Preferences.Get("IsCorporateClient", false);

            if (isCorporateClient)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Corporate purchase recorded. Your account will be settled at the end of the month.", "OK");
            }
            else
            {
                App.Database.ClearBasket(userId);
                BasketItems.Clear();
                OnPropertyChanged(nameof(TotalPrice));

                await Application.Current.MainPage.DisplayAlert("Success", "Your purchase has been completed!", "OK");
            }

            await Application.Current.MainPage.Navigation.PopAsync();
        }


    }
}