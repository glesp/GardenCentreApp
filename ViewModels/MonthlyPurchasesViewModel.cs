using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GardenCentreApp.Models;
using GardenCentreApp.Services;

namespace GardenCentreApp.ViewModels;

public partial class MonthlyPurchasesViewModel : ObservableObject
{
    private readonly LogoutService _logoutService;
    public ObservableCollection<BasketItem> CorporatePurchases { get; }

    public MonthlyPurchasesViewModel()
    {
        _logoutService = new LogoutService();
        var allCorporatePurchases = App.Database.GetCorporatePurchases();
        CorporatePurchases = new ObservableCollection<BasketItem>(
            allCorporatePurchases.Where(p => p.PurchaseDate.Month == DateTime.Now.Month &&
                                             p.PurchaseDate.Year == DateTime.Now.Year)
        );
    }
    
    [RelayCommand]
    private async Task Logout()
    {
        await _logoutService.LogoutAsync();
    }
}