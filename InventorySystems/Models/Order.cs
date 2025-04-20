using SQLite;
using InventorySystems.Models;


namespace InventorySystems.Models
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]

        public int OrderID { get; set; }            // Unique identifier for the order
        public DateTime OrderDate { get; set; }     // The date when the order was placed
        public int CustomerID { get; set; }         // Foreign key to the Customer table
        public int SupplierID { get; set; }         // Foreign key to the Supplier table
        public int ProductID { get; set; }          // Foreign key to the Product table
        public int Quantity { get; set; }           // Quantity of the product ordered
        public decimal Price { get; set; }          // Price of the product at the time of order
        public decimal SubTotal { get; set; }      // Subtotal (Quantity * Price)
        public decimal TotalAmount { get; set; }   // Total amount for the order (can be Sum of SubTotal for multiple products)
        public int UserID { get; set; }             // Foreign key to the UserAccount table, if applicable for the user creating the order

        public decimal CalculateSubTotal()
        {
            return Quantity * Price;
        }

        public decimal CalculateTotalAmount()
        {
            // If multiple items are in the order, this could aggregate the SubTotal for each item
            return SubTotal;
        }
    }
}
