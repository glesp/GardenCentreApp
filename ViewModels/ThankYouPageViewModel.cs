using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using GardenCentreApp.Models;

namespace GardenCentreApp.ViewModels
{
    public partial class ThankYouPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BasketItem> purchasedItems;

        public double TotalPrice => PurchasedItems.Sum(item => item.Product.Price * item.Quantity);

        public ThankYouPageViewModel(ObservableCollection<BasketItem> items)
        {
            PurchasedItems = items;
        }
    }
}