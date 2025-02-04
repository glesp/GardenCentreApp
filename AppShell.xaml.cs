namespace GardenCentreApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = this;  // Set Shell as the Binding Context
        GoToLoginPage();
        
        // // Explicitly enforce FlyoutBehavior
        // FlyoutBehavior = FlyoutBehavior.Flyout;
        //
        // // Remove any stray TabBar
        // foreach (var item in Items.ToList())
        // {
        //     if (item is TabBar)
        //         Items.Remove(item);
        // }
    }
    
    private async void GoToLoginPage()
    {
        await GoToAsync("//LoginPage");
    }

    public Command<string> GoToCategoryCommand => new Command<string>(async (category) =>
    {
        await Shell.Current.GoToAsync($"//Products?category={category}");
    });

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Perform Logout
        await Shell.Current.GoToAsync("//LoginPage");
    }
}