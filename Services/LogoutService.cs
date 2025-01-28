using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace GardenCentreApp.Services
{
    public class LogoutService
    {
        public async Task LogoutAsync()
        {
            // Clear preferences
            Preferences.Remove("UserId");
            Preferences.Remove("IsCorporateClient");

            // Navigate back to the LoginPage
            await Shell.Current.GoToAsync("//Login"); // Reset navigation stack and go to LoginPage
        }
    }
}