using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using GardenCentreApp.Models;

namespace GardenCentreApp.Pages
{
    public partial class BasketPage : ContentPage
    {
        private ObservableCollection<BasketItem> BasketItems;

        public BasketPage(int userId)
        {
            InitializeComponent();

            // Load basket items for the given userId from the database
            var itemsFromDb = App.Database.GetBasketItems(userId);
            BasketItems = new ObservableCollection<BasketItem>(itemsFromDb);

            BasketList.ItemsSource = BasketItems;
            UpdateTotalPrice();
        }

        private void OnRemoveItemClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var itemToRemove = button?.BindingContext as BasketItem;

            if (itemToRemove != null)
            {
                App.Database.RemoveBasketItem(itemToRemove.Id); // Remove from database
                BasketItems.Remove(itemToRemove); // Remove from ObservableCollection
                UpdateTotalPrice();
            }
        }

        private void UpdateTotalPrice()
        {
            double total = 0;

            // Calculate the total price
            foreach (var item in BasketItems)
            {
                var product = App.Database.GetProducts().FirstOrDefault(p => p.Id == item.ProductId);
                total += (product?.Price ?? 0) * item.Quantity;
            }

            TotalPriceLabel.Text = $"Total: €{total:F2}";
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            // Clear the user's basket after confirming checkout
            bool confirm = await DisplayAlert("Checkout", "Do you want to confirm your purchase?", "Yes", "No");
            if (confirm)
            {
                var userId = BasketItems.FirstOrDefault()?.UserId ?? 0;
                App.Database.ClearBasket(userId);

                BasketItems.Clear();
                UpdateTotalPrice();

                await DisplayAlert("Success", "Purchase completed!", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}
