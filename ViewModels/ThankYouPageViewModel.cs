using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GardenCentreApp.Models;
using GardenCentreApp.Services;

namespace GardenCentreApp.ViewModels
{
    
    public partial class ThankYouPageViewModel : ObservableObject
    {
        private readonly LogoutService _logoutService;

        [ObservableProperty]
        private ObservableCollection<BasketItem> purchasedItems;

        public double TotalPrice => PurchasedItems.Sum(item => item.Product.Price * item.Quantity);

        public ThankYouPageViewModel(ObservableCollection<BasketItem> items)
        {
            PurchasedItems = items;
            _logoutService = new LogoutService();
        }
        
        [RelayCommand]
        private async Task Logout()
        {
            await _logoutService.LogoutAsync();
        }
        
        
    }
}