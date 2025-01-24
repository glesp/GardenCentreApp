using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using GardenCentreApp.Models;
using GardenCentreApp.Pages;

namespace GardenCentreApp.ViewModels
{
    public partial class CategoriesPageViewModel : ObservableObject
    {
        // Observable property for the categories list
        [ObservableProperty]
        private ObservableCollection<string> categories;

        public CategoriesPageViewModel()
        {
            // Initialize categories
            Categories = new ObservableCollection<string>
            {
                "Plants",
                "Tools",
                "Garden Care",
                "Seeds",
                "Fertilizers"
            };
        }

        // Command for navigating to a specific category's product page
        [RelayCommand]
        private async void NavigateToCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
                return;

            // Navigate to the ProductPage with the selected category as a parameter
            await Application.Current.MainPage.Navigation.PushAsync(new ProductPage());
        }
    }
}