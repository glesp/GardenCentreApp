using GardenCentreApp.Models;
using Microsoft.Maui.Controls;

namespace GardenCentreApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var phoneNumber = PhoneEntry.Text;
            var password = PasswordEntry.Text;

            var user = App.Database.AuthenticateUser(phoneNumber, password);

            if (user != null)
            {
                await DisplayAlert("Login Successful", $"Welcome, {user.Name}!", "OK");
                await Navigation.PushAsync(new ProductPage());
            }
            else
            {
                await DisplayAlert("Error", "Invalid phone number or password.", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var phoneNumber = PhoneEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            var user = new User
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Password = password
            };

            if (App.Database.RegisterUser(user))
            {
                await DisplayAlert("Registration Successful", "You can now log in.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "User with this phone number already exists.", "OK");
            }
        }
    }
}