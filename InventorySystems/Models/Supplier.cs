using SQLite;
using System.Data;
using InventorySystems.Models;


namespace InventorySystems.Models
{
    public class Supplier
    {
        [PrimaryKey, AutoIncrement]
        public int SupplierID { get; set; }

        public string SupplierName { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Address { get; set; }

        public Supplier() { }

        public Supplier(int supplierid, string suppliername, string contactname, string contactemail, string contactphone, string address)
        {
            SupplierID = supplierid;
            SupplierName = suppliername;
            ContactName = contactname;
            ContactEmail = contactemail;
            ContactPhone = contactphone;
            Address = address;
        }
    }
}
