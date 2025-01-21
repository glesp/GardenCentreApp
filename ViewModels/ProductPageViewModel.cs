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
            // Access the database to load products
            Products = new ObservableCollection<Product>(App.Database.GetProducts());
        }

        [RelayCommand]
        private async void AddToBasket(Product product)
        {
            if (product == null) return;

            var userId = Preferences.Get("UserId", 0);
            var basketItem = new BasketItem
            {
                UserId = userId,
                ProductId = product.Id,
                Quantity = 1
            };

            App.Database.AddBasketItem(basketItem);
            await Application.Current.MainPage.DisplayAlert("Added to Basket", $"{product.Name} has been added to your basket.", "OK");
        }

        [RelayCommand]
        private async void ViewBasket()
        {
            var userId = Preferences.Get("UserId", 0);
            await Application.Current.MainPage.Navigation.PushAsync(new BasketPage(userId));
        }
    }
}