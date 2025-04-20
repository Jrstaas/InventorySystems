using SQLite;
using InventorySystems.Models;
public class Product
{
    [PrimaryKey, AutoIncrement]
    public int ProductID { get; set; }

    public string ProductName { get; set; }

    public string Description { get; set; }

    public string Category { get; set; }

    public decimal UnitPrice { get; set; }

    public int StockQuantity { get; set; }

    public int ReorderLevel { get; set; }


    // Add SupplierID for foreign key reference
    public int SupplierID { get; set; }
    public int UserID { get; set; }

    [Ignore] // This property will not be stored in the database, just for navigation
    public Supplier Supplier { get; set; } // Navigation property for Supplier

    public Product() { }

    public Product(string productName, string description, string category, decimal unitPrice, int stockQuantity, int reorderLevel)
    {
        ProductName = productName;
        Description = description;
        Category = category;
        UnitPrice = unitPrice;
        StockQuantity = stockQuantity;
        ReorderLevel = reorderLevel;
    }
}
