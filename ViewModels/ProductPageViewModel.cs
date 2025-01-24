using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using GardenCentreApp.Models;
using GardenCentreApp.Pages;

namespace GardenCentreApp.ViewModels
{
    public partial class ProductPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> products;

        public ObservableCollection<BasketItem> Basket { get; private set; } = new();
        
        
        public ProductPageViewModel()
        {
            // Load all products by default
            Products = new ObservableCollection<Product>(App.Database.GetProducts());
        }

        public ProductPageViewModel(string category)
        {
            Products = new ObservableCollection<Product>(
                App.Database.GetProducts().Where(p => p.Category == category)
            );
        }

        [RelayCommand]
        private void AddToBasket(Product product)
        {
            var userId = Preferences.Get("UserId", 0);
            var isCorporateClient = Preferences.Get("IsCorporateClient", false); // NEW: Get corporate flag

            var basketItem = new BasketItem
            {
                UserId = userId,
                ProductId = product.Id,
                Quantity = 1,
                IsCorporatePurchase = isCorporateClient // NEW: Mark as corporate purchase if applicable
            };

            App.Database.AddBasketItem(basketItem);
            Application.Current.MainPage.DisplayAlert("Added to Basket", $"{product.Name} has been added to your basket.", "OK");
        }


        [RelayCommand]
        private async void ViewBasket()
        {
            var userId = Preferences.Get("UserId", 0);
            await Application.Current.MainPage.Navigation.PushAsync(new BasketPage(userId));
        }
    }
}