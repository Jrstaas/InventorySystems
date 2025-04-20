using Microsoft.Maui.Controls;
using InventorySystems.Models;
using System.Collections.ObjectModel;
using InventorySystems.Data;
using System.IO;
/*using System.Net;
using System.Net.Mail;*/
using Microsoft.Maui.ApplicationModel.Communication;

namespace InventorySystems
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Product> products = new();
        private Query _query;
        private int _userId;
        private string _username;

        public MainPage(int userId, string username, Query query)
        {
            InitializeComponent();
            _userId = userId;
            _username = username;
            string dbPath = System.IO.Path.Combine(Environment.GetEnvironmentVariable("AppData"), "inventsys.db");
            _query = new Query(dbPath);
            LoadProducts();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = e.NewTextValue?.ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                ProductCollectionView.ItemsSource = products;
            }
            else
            {
                var filtered = products.Where(p =>
                    (p.ProductName?.ToLower().Contains(keyword) ?? false) ||
                    (p.Description?.ToLower().Contains(keyword) ?? false) ||
                    (p.Category?.ToLower().Contains(keyword) ?? false)).ToList();

                ProductCollectionView.ItemsSource = filtered;
            }
        }

        private async void LoadProducts()
        {
            var allProducts = await _query.GetAllProductsAsync();
            var userProducts = allProducts.Where(p => p.UserID == _userId).ToList();
            products = new ObservableCollection<Product>(userProducts);
            ProductCollectionView.ItemsSource = products;
        }


        private async void AddProductClicked(object sender, EventArgs e)
        {
            var name = ProductNameEntry.Text?.Trim();
            var description = DescriptionEntry.Text?.Trim();
            var category = CategoryEntry.Text?.Trim();
            //var supplierId = int.TryParse(SupplierIdEntry.Text, out var sId) ? sId : 0;//
            var unitPrice = decimal.TryParse(UnitPriceEntry.Text, out var price) ? price : 0;
            var quantity = int.TryParse(StockQuantityEntry.Text, out var qty) ? qty : 0;

            if (string.IsNullOrWhiteSpace(name)) return;

            var newProduct = new Product
            {
                ProductName = name,
                Description = description,
                Category = category,
                UnitPrice = unitPrice,
                StockQuantity = quantity,
                //SupplierID = supplierId,//
                UserID = _userId
            };

            await _query.AddProductToDatabaseAsync(newProduct);
            LoadProducts();
            ProductNameEntry.Text = "";
            DescriptionEntry.Text = "";
            CategoryEntry.Text = "";
            //SupplierIdEntry.Text = "";//
            UnitPriceEntry.Text = "";
            StockQuantityEntry.Text = "";
        }

        private async void AdjustValue(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Product product)
            {
                var parent = button.Parent as StackLayout;
                var quantityEntry = parent?.Children
                    .OfType<Entry>()
                    .FirstOrDefault(e => e.ClassId == "QuantityEntry");

                int quantity = 1;
                if (quantityEntry != null && int.TryParse(quantityEntry.Text, out int parsedQty))
                {
                    quantity = parsedQty;
                }

                if (quantity <= 0)
                {
                    await DisplayAlert("Invalid Quantity", "Please enter a valid quantity.", "OK");
                    return;
                }

                bool isBuy = button.Text.Equals("Buy", StringComparison.OrdinalIgnoreCase);
                int delta = isBuy ? quantity : -quantity;

                if (!isBuy && product.StockQuantity < quantity)
                {
                    await DisplayAlert("Error", "Not enough stock to sell.", "OK");
                    return;
                }

                product.StockQuantity += delta;

                await _query.UpdateProductInDatabaseAsync(product);

                if (!isBuy)
                {
                    // Fetch all users excluding the current user
                    var users = await _query.GetUsersAsync();
                    var otherUsers = users.Where(u => u.UserID != _userId && u.UserID != 99).ToList();
                    var userOptions = otherUsers.Select(u => u.Username).ToList();
                    userOptions.Add("Other");

                    // Display a list of users for selection
                    var selectedUsername = await DisplayActionSheet("Select Customer", "Cancel", null, userOptions.ToArray());

                    if (selectedUsername == "Cancel" || string.IsNullOrWhiteSpace(selectedUsername))
                    {
                        return;
                    }

                    if (selectedUsername == "Other")
                    {
                        await DisplayAlert("Success", isBuy ? "Purchase complete!" : "Sale recorded to Outside Customer!", "OK");
                    }
                    else
                    {
                        var buyer = otherUsers.FirstOrDefault(u => u.Username == selectedUsername);
                        if (buyer == null)
                        {
                            await DisplayAlert("Error", "Selected user not found.", "OK");
                            return;
                        }
                        // Check if the buyer already has this product
                        var buyerProduct = await _query.GetProductByUserIdAndProductIdAsync(buyer.UserID, product.ProductID);
                        if (buyerProduct == null)
                        {
                            // Create a new product entry for the buyer
                            buyerProduct = new Product
                            {
                                ProductName = product.ProductName,
                                Description = product.Description,
                                Category = product.Category,
                                UnitPrice = product.UnitPrice,
                                StockQuantity = quantity,
                                UserID = buyer.UserID
                            };

                            await _query.AddProductToDatabaseAsync(buyerProduct);
                        }
                        else
                        {
                            // Update existing product for the buyer
                            buyerProduct.StockQuantity += quantity;
                            await _query.UpdateProductInDatabaseAsync(buyerProduct);
                        }

                        var order = new Order
                        {
                            OrderDate = DateTime.Now,
                            ProductID = product.ProductID,
                            Quantity = quantity,
                            Price = product.UnitPrice,
                            SubTotal = product.UnitPrice * quantity,
                            TotalAmount = product.UnitPrice * quantity,
                            UserID = _userId,
                            SupplierID = product.SupplierID
                        };

                        await _query.InsertOrderAsync(order);
                        //await SendOrderEmailAsync(order, product);
                        await DisplayAlert("Success", isBuy ? "Purchase complete!" : "Sale recorded!", "OK");
                    }
                }
                        LoadProducts();
            }
        }



        private async void DeleteProductClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Product product)
            {
                await _query.DeleteProductAsync(product.ProductID);
                LoadProducts();
            }
        }

        /*The emailing, in order to automatically send, is a security concern, for simplicity of demonstration
        I will remove it
        private async Task SendOrderEmailAsync(Order order, Product product)
        {
            if (Email.Default.IsComposeSupported)
            {
                string subject = "Order From InventorySystems Inc!";
                string body = $"Order Confirmation\n" +
                              $"Product: {product.ProductName}\n" +
                              $"Description: {product.Description}\n" +
                              $"Category: {product.Category}\n" +
                              $"Price per Unit: {product.UnitPrice:C}\n" +
                              $"Quantity: {order.Quantity}\n" +
                              $"Total: {order.TotalAmount:C}\n" +
                              $"Date: {order.OrderDate:g}\n\n" +
                              $"Thank you for using InventorySystems.";

                string recipients = "2810285@vikes.csuohio.edu";

                // Encode subject and body to ensure proper URL formatting
                string encodedSubject = Uri.EscapeDataString(subject);
                string encodedBody = Uri.EscapeDataString(body);

                // Construct the mailto URI
                var mailtoUri = new Uri($"mailto:{recipients}?subject={encodedSubject}&body={encodedBody}");

                // Open the default email client with the composed message
                await Launcher.Default.OpenAsync(mailtoUri);
            }
        }*/

    }
}
