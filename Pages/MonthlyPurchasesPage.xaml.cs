using GardenCentreApp.ViewModels;

namespace GardenCentreApp.Pages;

public partial class MonthlyPurchasesPage : ContentPage
{
    public MonthlyPurchasesPage()
    {
        InitializeComponent();
        BindingContext = new MonthlyPurchasesViewModel();
    }
}