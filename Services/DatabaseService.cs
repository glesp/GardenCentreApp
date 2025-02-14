using GardenCentreApp.Models;
using SQLite;

namespace GardenCentreApp.Services
{
    public class DatabaseService
    {
        private SQLiteConnection _database;

        public DatabaseService()
        {
            // Define the database path with platform-specific logic
            string dbPath;
#if ANDROID
            // Use LocalApplicationData for Android
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gardencentre.db");
#else
            // Default path for other platforms
            dbPath = Path.Combine(FileSystem.AppDataDirectory, "gardencentre.db");
#endif
            // Debug the database path for troubleshooting
            System.Diagnostics.Debug.WriteLine($"Database Path: {dbPath}");

            // Initialize the SQLite connection
            _database = new SQLiteConnection(dbPath);

            // Create the necessary tables
            _database.CreateTable<User>();
            _database.CreateTable<Product>();
            _database.CreateTable<BasketItem>();
            
            SeedProducts();
        }

        // ---------------------------
        // User Management
        // ---------------------------
        public bool RegisterUser(User user)
        {
            // Check if the user already exists based on phone number
            if (_database.Table<User>().Any(u => u.PhoneNumber == user.PhoneNumber))
            {
                return false; // User already exists
            }
            _database.Insert(user); // Insert new user
            return true;
        }

        public User AuthenticateUser(string phoneNumber, string password)
        {
            // Find the user by phone number and password
            return _database.Table<User>().FirstOrDefault(u => u.PhoneNumber == phoneNumber && u.Password == password);
        }

        // ---------------------------
        // Product Management
        // ---------------------------
        public void SeedProducts()
        {
            if (!_database.Table<Product>().Any())
            {
                var defaultProducts = new List<Product>
                {
                    new Product { Name = "Rose Plant", Category = "Plants", Price = 10, Image = "rose.png" },
                    new Product { Name = "Garden Shovel", Category = "Tools", Price = 25, Image = "shovel.png" },
                    new Product { Name = "Fertilizer", Category = "Garden Care", Price = 15, Image = "fertilizer.png" },
                    new Product { Name = "Lily Plant", Category = "Plants", Price = 20, Image = "lily.jpg" },
                    new Product { Name = "Garden Hoe", Category = "Tools", Price = 30, Image = "hoe.webp" },
                    new Product { Name = "Pesticide", Category = "Garden Care", Price = 10, Image = "pesticide.webp" },
                    new Product { Name = "Sunflower Plant", Category = "Plants", Price = 15, Image = "sunflower.jpg" },
                    new Product { Name = "Garden Rake", Category = "Tools", Price = 20, Image = "rake.jpg" },
                    new Product { Name = "Weed Killer", Category = "Garden Care", Price = 10, Image = "weedkiller.jpg" },
                    new Product { Name = "Tulip Plant", Category = "Plants", Price = 10, Image = "tulip.jpg" },
                    new Product { Name = "Garden Shears", Category = "Tools", Price = 25, Image = "shears.jpg" },
                    new Product { Name = "Compost", Category = "Garden Care", Price = 15, Image = "compost.jpg" },
                    new Product { Name = "Daisy Plant", Category = "Plants", Price = 20, Image = "daisy.jpg" },
                    new Product { Name = "Garden Fork", Category = "Tools", Price = 30, Image = "fork.png" },
                    new Product { Name = "Mulch", Category = "Garden Care", Price = 10, Image = "mulch.jpg" },
                    new Product { Name = "Orchid Plant", Category = "Plants", Price = 15, Image = "orchid.jpg" },
                    new Product { Name = "Garden Trowel", Category = "Tools", Price = 20, Image = "trowel.jpg" },
                   
                };

                _database.InsertAll(defaultProducts);
            }
        }

        public void AddProduct(Product product)
        {
            // Add a new product to the database
            _database.Insert(product);
        }

        public void DeleteProduct(Product product)
        {
            // Delete a product from the database
            _database.Delete(product);
        }

        public List<Product> GetProducts()
        {
            // Retrieve all products from the database
            return _database.Table<Product>().ToList();
        }

        public void ClearProducts()
        {
            // Delete all products (useful for testing)
            _database.DeleteAll<Product>();
        }

        // ---------------------------
        // Basket Management
        // ---------------------------
        public void AddBasketItem(BasketItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Basket item cannot be null.");

            if (item.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(item.Quantity));

            // Check if the item already exists in the basket for the same user and product
            var existingItem = _database.Table<BasketItem>()
                .FirstOrDefault(b => b.UserId == item.UserId && b.ProductId == item.ProductId && b.IsCorporatePurchase == item.IsCorporatePurchase);

            if (existingItem != null)
            {
                // If the item already exists, update the quantity and timestamp
                existingItem.Quantity += item.Quantity;
                existingItem.PurchaseDate = DateTime.Now; // Update the timestamp
                _database.Update(existingItem);
            }
            else
            {
                // If it's a new item, add it to the database
                item.PurchaseDate = DateTime.Now; // Set the timestamp
                _database.Insert(item);
            }
        }


        public List<BasketItem> GetCorporatePurchases()
        {
            return _database.Table<BasketItem>().Where(b => b.IsCorporatePurchase).ToList();
        }

        public List<BasketItem> GetBasketItems(int userId)
        {
            // Retrieve all basket items for a specific user
            return _database.Table<BasketItem>().Where(b => b.UserId == userId).ToList();
        }

        public List<BasketItem> GetBasketItemsWithProducts(int userId)
        {
            // Retrieve all basket items with product details for a specific user
            var items = _database.Table<BasketItem>().Where(b => b.UserId == userId).ToList();
            foreach (var item in items)
            {
                // Attach the associated product to each basket item
                item.Product = _database.Table<Product>().FirstOrDefault(p => p.Id == item.ProductId);
            }
            return items;
        }

        public void RemoveBasketItem(int basketItemId)
        {
            // Remove a specific item from the basket
            var item = _database.Table<BasketItem>().FirstOrDefault(b => b.Id == basketItemId);
            if (item != null)
            {
                _database.Delete(item);
            }
        }

        public void ClearBasket(int userId)
        {
            // Remove all items from the basket for a specific user
            var userBasketItems = _database.Table<BasketItem>().Where(b => b.UserId == userId).ToList();
            foreach (var item in userBasketItems)
            {
                _database.Delete(item);
            }
        }
    }
}
