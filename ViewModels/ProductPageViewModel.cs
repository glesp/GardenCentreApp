using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using GardenCentreApp.Models;
using GardenCentreApp.Pages;

namespace GardenCentreApp.ViewModels
{
    public partial class ProductPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        private ObservableCollection<string> categories;

        [ObservableProperty]
        private string selectedCategory;

        public ProductPageViewModel()
        {
            // Initialize categories
            Categories = new ObservableCollection<string>
            {
                "All",
                "Plants",
                "Tools",
                "Garden Care",
                "Seeds",
                "Fertilizers"
            };

            // Load all products by default
            LoadProducts("All");
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                LoadProducts(value);
            }
        }

        private void LoadProducts(string category)
        {
            if (category == "All")
            {
                Products = new ObservableCollection<Product>(App.Database.GetProducts());
            }
            else
            {
                Products = new ObservableCollection<Product>(
                    App.Database.GetProducts().Where(p => p.Category == category)
                );
            }
        }
        
        [RelayCommand]
        private void ChangeCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return;

            SelectedCategory = category;
            Products = new ObservableCollection<Product>(
                App.Database.GetProducts().Where(p => p.Category == category || category == "All")
            );
        }
        
        [RelayCommand]
        private async Task GoToBasket()
        {
            var userId = Preferences.Get("UserId", 0); // Retrieve the user's ID
            await Application.Current.MainPage.Navigation.PushAsync(new BasketPage(userId));
        }



        [RelayCommand]
        private void AddToBasket(Product product)
        {
            var userId = Preferences.Get("UserId", 0);
            var isCorporateClient = Preferences.Get("IsCorporateClient", false);

            var basketItem = new BasketItem
            {
                UserId = userId,
                ProductId = product.Id,
                Quantity = 1,
                IsCorporatePurchase = isCorporateClient
            };

            App.Database.AddBasketItem(basketItem);
            Application.Current.MainPage.DisplayAlert("Added to Basket", $"{product.Name} has been added to your basket.", "OK");
        }
    }
}
