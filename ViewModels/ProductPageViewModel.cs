using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using GardenCentreApp.Models;
using GardenCentreApp.Pages;
using GardenCentreApp.Services;

namespace GardenCentreApp.ViewModels
{
    public partial class ProductPageViewModel : ObservableObject
    {
        private readonly LogoutService _logoutService;
        
        [ObservableProperty]
        private string pageTitle;
        
        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        private ObservableCollection<string> categories;

        [ObservableProperty]
        private string selectedCategory;

        public ProductPageViewModel()
        {   
            _logoutService = new LogoutService();
            // Initialize categories
            Categories = new ObservableCollection<string>
            {
                "All",
                "Plants",
                "Tools",
                "Garden Care",
            };
            SelectedCategory = "All"; // Default category
            PageTitle = "All Products"; // Default title

            // Load all products by default
            LoadProducts("All");
        }
        
        [RelayCommand]
        private async Task Logout()
        {
            await _logoutService.LogoutAsync();
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                PageTitle = value == "All" ? "All Products" : $"{value}"; // Dynamically update title
                LoadProducts(value);
            }
        }

        
        // Navigate left (to the previous item in the carousel) with cyclic behavior
        [RelayCommand]
        private void MoveLeft()
        {
            var currentIndex = Categories.IndexOf(SelectedCategory);
            if (currentIndex > 0)
            {
                // Move to previous category
                SelectedCategory = Categories[currentIndex - 1];
            }
            else
            {
                // If we're at the first item, cycle to the last item
                SelectedCategory = Categories.Last();
            }
            LoadProducts(SelectedCategory);
        }

        // Navigate right (to the next item in the carousel) with cyclic behavior
        [RelayCommand]
        private void MoveRight()
        {
            var currentIndex = Categories.IndexOf(SelectedCategory);
            if (currentIndex < Categories.Count - 1)
            {
                // Move to next category
                SelectedCategory = Categories[currentIndex + 1];
            }
            else
            {
                // If we're at the last item, cycle to the first item
                SelectedCategory = Categories.First();
            }
            LoadProducts(SelectedCategory);
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
        
        // [RelayCommand]
        // private async Task GoToBasket()
        // {
        //     var userId = Preferences.Get("UserId", 0); // Retrieve the user's ID
        //     await Application.Current.MainPage.Navigation.PushAsync(new BasketPage(userId));
        // }

        [RelayCommand]
        private void IncreaseQuantity(Product product)
        {
            if (product != null)
            {
                product.Quantity++;
            }
        }
        
        public void UpdateCategoryFromRoute(string route)
        {
            string category = route.Contains("Plants") ? "Plants" :
                route.Contains("Tools") ? "Tools" :
                route.Contains("GardenCare") ? "Garden Care" : "All";

            SelectedCategory = category; // This triggers `OnSelectedCategoryChanged`
        }


        [RelayCommand]
        private void DecreaseQuantity(Product product)
        {
            if (product != null && product.Quantity > 1)
            {
                product.Quantity--;
            }
        }

        [RelayCommand]
        private void AddToBasket(Product product)
        {
            if (product == null)
            {
                Application.Current.MainPage.DisplayAlert("Error", "No product selected.", "OK");
                return;
            }

            var userId = Preferences.Get("UserId", 0);
            var isCorporateClient = Preferences.Get("IsCorporateClient", false);

            var basketItem = new BasketItem
            {
                UserId = userId,
                ProductId = product.Id,
                Quantity = product.Quantity, // Use the selected quantity
                IsCorporatePurchase = isCorporateClient
            };

            try
            {
                // Call the database service
                App.Database.AddBasketItem(basketItem);


                // Show success message
                Application.Current.MainPage.DisplayAlert(
                    "Added to Basket",
                    $"{product.Quantity}x {product.Name} has been added to your basket.",
                    "OK"
                );
                product.Quantity = 1; // <-- This line is added to reset the quantity
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., invalid quantity)
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        

    }
}
