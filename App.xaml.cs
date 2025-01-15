using GardenCentreApp.Pages;
using GardenCentreApp.Services;
using Microsoft.Maui.Controls;

namespace GardenCentreApp
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();
            Database = new DatabaseService();
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}