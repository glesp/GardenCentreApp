using System.Collections.ObjectModel;
using GardenCentreApp.Models;
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
        await Application.Current.MainPage.Navigation.PopToRootAsync(); // Navigate back to ProductPage
    }
}