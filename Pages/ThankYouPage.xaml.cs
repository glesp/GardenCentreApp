using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GardenCentreApp.Models;
using GardenCentreApp.Services;
using GardenCentreApp.ViewModels;
using Microsoft.Maui.Controls;

namespace GardenCentreApp.Pages;

public partial class ThankYouPage : ContentPage
{

    public ThankYouPage(ObservableCollection<BasketItem> items)
    {
        InitializeComponent();
        BindingContext = new ThankYouPageViewModel(items); // Pass data to ViewModel
    }

    private async void OnReturnToProductsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Products"); // Navigate to the ProductPage
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Shell.SetTabBarIsVisible(this, false); // Hide bottom tabs
    }
    
}