using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GardenCentreApp.Models;
using GardenCentreApp.Services;
using Microsoft.Maui.Controls;

namespace GardenCentreApp.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorMessage;
        
        [ObservableProperty]
        private bool isCorporateClient;
        
        
        partial void OnIsCorporateClientChanged(bool value)
        {
            System.Diagnostics.Debug.WriteLine($"IsCorporateClient changed to: {value}");
        }

        public LoginPageViewModel()
        {
            PhoneNumber = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }
        
        [RelayCommand]
        private async void Login()
        {
            var user = App.Database.AuthenticateUser(PhoneNumber, Password);

            if (user != null)
            {
                Preferences.Set("UserId", user.Id);
                Preferences.Set("IsCorporateClient", IsCorporateClient); // NEW: Store corporate flag
                System.Diagnostics.Debug.WriteLine($"Preferences saved: IsCorporateClient = {IsCorporateClient}");
                await Application.Current.MainPage.Navigation.PushAsync(new Pages.ProductPage());
            }
            else
            {
                ErrorMessage = "Invalid phone number or password.";
            }
        }


        [RelayCommand]
        private async void Register()
        {
            // Navigate to a RegistrationPage (not implemented yet, optional)
            await Application.Current.MainPage.DisplayAlert("Register", "Registration functionality is not implemented yet.", "OK");
        }
    }
}