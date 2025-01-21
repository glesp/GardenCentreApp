using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GardenCentreApp.Models;

namespace GardenCentreApp.ViewModels;

public class MonthlyPurchasesViewModel : ObservableObject
{
    public ObservableCollection<BasketItem> CorporatePurchases { get; }

    public MonthlyPurchasesViewModel()
    {
        var allCorporatePurchases = App.Database.GetCorporatePurchases();
        CorporatePurchases = new ObservableCollection<BasketItem>(
            allCorporatePurchases.Where(p => p.PurchaseDate.Month == DateTime.Now.Month &&
                                             p.PurchaseDate.Year == DateTime.Now.Year)
        );
    }
}