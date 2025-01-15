using SQLite;

namespace GardenCentreApp.Models
{
    public class BasketItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Unique identifier for the basket item
        public int UserId { get; set; } // Links the basket to a specific user
        public int ProductId { get; set; } // Links to the product in the database
        
        public int Price { get; set; } // Price of the product at the time it was added to the basket
        public int Quantity { get; set; } // Quantity of the product in the basket
    }
}
