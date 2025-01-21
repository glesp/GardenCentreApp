using SQLite;
using System.Collections.Generic;
using System.IO;
using GardenCentreApp.Models;

namespace GardenCentreApp.Services
{
    public class DatabaseService
    {
        private SQLiteConnection _database;

        public DatabaseService()
        {
            // Define the database path
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "gardencentre.db");

            // Debug the database path for troubleshooting
            System.Diagnostics.Debug.WriteLine($"Database Path: {dbPath}");

            // Initialize the SQLite connection
            _database = new SQLiteConnection(dbPath);

            // Create the necessary tables
            _database.CreateTable<User>();
            _database.CreateTable<Product>();
            _database.CreateTable<BasketItem>();
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
            // Check if the item already exists in the user's basket
            var existingItem = _database.Table<BasketItem>()
                .FirstOrDefault(b => b.UserId == item.UserId && b.ProductId == item.ProductId);

            if (existingItem != null)
            {
                // Update the quantity if the item exists
                existingItem.Quantity += item.Quantity;
                _database.Update(existingItem);
            }
            else
            {
                // Add a new item if it doesn't exist
                _database.Insert(item);
            }
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
