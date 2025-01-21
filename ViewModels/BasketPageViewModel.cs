using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
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
    }
}