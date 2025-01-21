using SQLite;

namespace GardenCentreApp.Models
{
    public class BasketItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool IsCorporatePurchase { get; set; }

        [Ignore] // This property is not stored in the database
        public Product Product { get; set; }
    }
}