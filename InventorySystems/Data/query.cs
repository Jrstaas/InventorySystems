using System;
using System.Data;
using SQLite;
using System.Collections.Generic;
using InventorySystems.Models;
using System.Threading.Tasks;

namespace InventorySystems.Data
{
    public class Query
    {
        private SQLiteAsyncConnection _connection;

        public Query(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath); // Use SQLiteAsyncConnection
            _connection.CreateTableAsync<Product>().Wait();
            _connection.CreateTableAsync<Order>().Wait();
            _connection.CreateTableAsync<Customer>().Wait();
            _connection.CreateTableAsync<Supplier>().Wait();
            _connection.CreateTableAsync<User>().Wait();
        }

        //------PRODUCT CRUD Operations---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task AddProductToDatabaseAsync(Product product)
        {
            await _connection.InsertAsync(product); // Async insert
        }

        public async Task<Product> SelectProductInDatabaseAsync(int productID)
        {
            return await _connection.Table<Product>().FirstOrDefaultAsync(p => p.ProductID == productID); // Async version
        }

        public async Task UpdateProductInDatabaseAsync(Product product)
        {
            await _connection.UpdateAsync(product); // Async update
        }

        public async Task DeleteProductInDatabaseAsync(int productID)
        {
            var product = await _connection.Table<Product>().FirstOrDefaultAsync(p => p.ProductID == productID); // Async fetch
            if (product != null)
            {
                await _connection.DeleteAsync(product); // Async delete
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _connection.Table<Product>().ToListAsync();
        }


        //------ORDER CRUD Operations---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task InsertOrderAsync(Order order)
        {
            order.OrderID = 1;
            order.CustomerID = 1;
            order.SupplierID = 1;
            await _connection.InsertAsync(order); // Async insert
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _connection.UpdateAsync(order); // Async update
        }

        public async Task DeleteOrderAsync(Order order)
        {
            await _connection.DeleteAsync(order); // Async delete
        }

        //------CUSTOMER CRUD Operations---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task AddCustomerToDatabaseAsync(Customer customer)
        {
            await _connection.InsertAsync(customer); // Async insert
        }

        public async Task UpdateCustomerInDatabaseAsync(Customer customer)
        {
            await _connection.UpdateAsync(customer); // Async update
        }

        public async Task DeleteCustomerFromDatabaseAsync(int customerID)
        {
            var customer = await _connection.Table<Customer>().FirstOrDefaultAsync(c => c.CustomerID == customerID); // Async fetch
            if (customer != null)
            {
                await _connection.DeleteAsync(customer); // Async delete
            }
        }

        public async Task<Customer> SelectCustomerInDatabaseAsync(int customerID)
        {
            return await _connection.Table<Customer>().FirstOrDefaultAsync(c => c.CustomerID == customerID); // Async version
        }

        //------SUPPLIER CRUD Operations---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task AddSupplierToDatabaseAsync(Supplier supplier)
        {
            await _connection.InsertAsync(supplier); // Async insert
        }

        public async Task<Supplier> SelectSupplierInDatabaseAsync(int supplierID)
        {
            return await _connection.Table<Supplier>().FirstOrDefaultAsync(s => s.SupplierID == supplierID); // Async version
        }

        public async Task UpdateSupplierInDatabaseAsync(Supplier supplier)
        {
            await _connection.UpdateAsync(supplier); // Async update
        }

        public async Task DeleteSupplierFromDatabaseAsync(int supplierID)
        {
            var supplier = await _connection.Table<Supplier>().FirstOrDefaultAsync(s => s.SupplierID == supplierID); // Async fetch
            if (supplier != null)
            {
                await _connection.DeleteAsync(supplier); // Async delete
            }
        }

        //------Get User Products---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<Product>> GetUserProductsAsync(int userId)
        {
            return await _connection.Table<Product>().Where(p => p.UserID == userId).ToListAsync(); // Async version
        }

        public async Task InsertOrUpdateSupplierAsync(Supplier supplier)
        {
            var query = "INSERT OR REPLACE INTO Supplier (SupplierName, ContactName, ContactEmail, ContactPhone, Address) VALUES (?, ?, ?, ?, ?)";
            await _connection.ExecuteAsync(query, supplier.SupplierName, supplier.ContactName, supplier.ContactEmail, supplier.ContactPhone, supplier.Address);
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            var query = "SELECT * FROM Supplier"; // Adjust query as needed for your database schema
            return await _connection.QueryAsync<Supplier>(query); // Async query
        }

        public async Task<Supplier> GetSupplierDetailsAsync(int supplierId)
        {
            var query = "SELECT * FROM Supplier WHERE SupplierID = ?";
            var suppliers = await _connection.QueryAsync<Supplier>(query, supplierId);
            return suppliers.FirstOrDefault();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _connection.Table<User>().ToListAsync();
            Console.WriteLine($"Found {users.Count} users in the database.");
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Username}, PasswordHash: {user.PasswordHash}");
            }
            return users;
        }


        public async Task InsertUserAsync(User user)
        {
            await _connection.InsertAsync(user); // Async insert
        }
        // In your Query class

        public async Task DeleteUserAsync(User user)
        {
            await _connection.DeleteAsync(user); // Delete the user asynchronously
        }

        // Validate the user login against the database (no password hashing)
        public async Task<User> ValidateUserLoginAsync(string username, string password)
        {
            // Retrieve all users from the database
            var users = await _connection.Table<User>().ToListAsync();

            // Make both the username and password case-insensitive
            var matchingUsers = users.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.PasswordHash == password).ToList();

            if (matchingUsers.Count > 0)
            {
                var user = matchingUsers[0]; // Assuming only one match is expected
                return user;
            }
            else
            {
                return null; // No matching user found
            }
        }
    }
}