using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace GardenCentreApp.Models
{
    public partial class Product : ObservableObject
    {   
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }

        [ObservableProperty] 
        private int quantity = 1;
    }
}