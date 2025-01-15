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
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "gardencentre.db");
            System.Diagnostics.Debug.WriteLine($"Database Path: {dbPath}");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<User>();
            _database.CreateTable<Product>();
            _database.CreateTable<BasketItem>();
        }

        // Register a new user
        public bool RegisterUser(User user)
        {
            if (_database.Table<User>().Any(u => u.PhoneNumber == user.PhoneNumber))
            {
                return false; // User already exists
            }
            _database.Insert(user);
            return true;
        }

        // Verify login credentials
        public User AuthenticateUser(string phoneNumber, string password)
        {
            return _database.Table<User>().FirstOrDefault(u => u.PhoneNumber == phoneNumber && u.Password == password);
        }
        
        // Add a new product to the database
        public void AddProduct(Product product)
        {
            // Insert the product into the database
            _database.Insert(product);
        }
        
        //Delete a product from the database
        public void DeleteProduct(Product product)
        {
            // Delete the product from the database
            _database.Delete(product);
        }

        // Retrieve all products from the database
        public List<Product> GetProducts()
        {
            // Query the Product table and return all rows as a list
            return _database.Table<Product>().ToList();
        }

        // Clear all products (optional utility for testing or resetting)
        public void ClearProducts()
        {
            _database.DeleteAll<Product>();
        }
        
        public void AddBasketItem(BasketItem item)
        {
            // Check if the item already exists for the user and product
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
            // Retrieve all items in the user's basket
            return _database.Table<BasketItem>().Where(b => b.UserId == userId).ToList();
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
            // Remove all items for a specific user
            var userBasketItems = _database.Table<BasketItem>().Where(b => b.UserId == userId).ToList();
            foreach (var item in userBasketItems)
            {
                _database.Delete(item);
            }
        }

    }
}