using SQLite;
using InventorySystems.Models;


namespace InventorySystems.Models
{
    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Address { get; set; }

        public string ContactName { get; set; }
    }
}
