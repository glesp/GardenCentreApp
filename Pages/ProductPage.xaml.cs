using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using GardenCentreApp.Models;

namespace GardenCentreApp.Pages
{
    public partial class ProductPage : ContentPage
    {
        private ObservableCollection<Product> Products = new ObservableCollection<Product>();

        public ProductPage()
        {
            InitializeComponent();
            LoadProducts();
            ProductList.ItemsSource = Products;
        }

        private void LoadProducts()
        {
            // Populate products from the database
            var productsFromDb = App.Database.GetProducts();
            Products.Clear();
            foreach (var product in productsFromDb)
            {
                Products.Add(product);
            }
        }

        private void OnAddToBasketClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedProduct = button?.BindingContext as Product;

            if (selectedProduct != null)
            {
                var userId = Preferences.Get("UserId", 0); // Assuming user ID is stored in Preferences
                var basketItem = new BasketItem
                {
                    UserId = userId,
                    ProductId = selectedProduct.Id,
                    Quantity = 1
                };

                App.Database.AddBasketItem(basketItem);
                DisplayAlert("Added to Basket", $"{selectedProduct.Name} has been added to your basket.", "OK");
            }
        }

        private async void OnViewBasketClicked(object sender, EventArgs e)
        {
            var userId = Preferences.Get("UserId", 0);
            await Navigation.PushAsync(new BasketPage(userId));
        }
    }
}