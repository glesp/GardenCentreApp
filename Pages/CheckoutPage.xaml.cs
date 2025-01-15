using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using GardenCentreApp.Models;

namespace GardenCentreApp.Pages
{
    public partial class CheckoutPage : ContentPage
    {
        public CheckoutPage(ObservableCollection<Product> products)
        {
            InitializeComponent();

            double total = 0;
            foreach (var product in products)
            {
                total += product.Price;
            }

            BasketList.ItemsSource = products;
            TotalLabel.Text = $"Total: {total:C}";
        }
    }
}