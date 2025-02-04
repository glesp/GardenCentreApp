

using GardenCentreApp.ViewModels;

namespace GardenCentreApp.Pages;

public partial class MonthlyPurchasesPage : ContentPage
{
    public MonthlyPurchasesPage()
    {
        InitializeComponent();
        BindingContext = new MonthlyPurchasesViewModel();
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Shell.SetTabBarIsVisible(this, false); // Hide bottom tabs
    }
}