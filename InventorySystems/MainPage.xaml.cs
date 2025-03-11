using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventorySystems
{
    public partial class MainPage : ContentPage
    {
        private View mainLayout;

        // List to hold item data (name, image, and value)
        private List<Item> Items;

        public MainPage(string username)
        {
            InitializeComponent();
            mainLayout = this.Content; // Save the initial layout
            WelcomeLabel.Text = $"Welcome, {username}!"; // Display username in welcome message

            // Initialize sample data (name, image, and value)
            Items = new List<Item>
            {
                new Item("Adafruit DAC6578", "dac6578.jpg", 0),
                new Item("Raspberry Pi Pico 2", "pico2.jpg", 0),
                new Item("Metro M4", "metro.jpg", 0),
                new Item("ItsyBitsy M0", "itsy.jpg", 0),
                new Item("Breadboard Wires", "wires.jpg", 0),
                new Item("Item 6", "https://via.placeholder.com/40", 0),
                new Item("Item 7", "https://via.placeholder.com/40", 0),
                new Item("Item 8", "https://via.placeholder.com/40", 0),
                new Item("Item 9", "https://via.placeholder.com/40", 0),
                new Item("Item 10", "https://via.placeholder.com/40", 0),
                new Item("Item 11", "https://via.placeholder.com/40", 0),
                new Item("Item 12", "https://via.placeholder.com/40", 0),
                new Item("Item 13", "https://via.placeholder.com/40", 0),
                new Item("Item 14", "https://via.placeholder.com/40", 0),
                new Item("Item 15", "https://via.placeholder.com/40", 0),
                new Item("Item 16", "https://via.placeholder.com/40", 0),
                new Item("Item 17", "https://via.placeholder.com/40", 0),
                new Item("Item 18", "https://via.placeholder.com/40", 0),
                new Item("Item 19", "https://via.placeholder.com/40", 0),
                new Item("Item 20", "https://via.placeholder.com/40", 0)
            };

            // Display initial grid
            PopulateGrid(Items);
        }

        // Method to populate the grid with items
        private void PopulateGrid(List<Item> items)
        {
            ItemGridContent.Children.Clear(); // Clear existing content

            int row = 0, col = 0;

            // Ensure the ItemGridContent grid has enough rows and columns
            ItemGridContent.RowDefinitions.Clear();
            ItemGridContent.ColumnDefinitions.Clear();

            // Create 8 rows and 5 columns based on the grid size
            for (int i = 0; i < 8; i++) // Assuming 8 rows
            {
                ItemGridContent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            for (int i = 0; i < 5; i++) // Assuming 5 columns
            {
                ItemGridContent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            foreach (var item in items)
            {
                if (col == 5) // Move to next row after 5 items
                {
                    col = 0;
                    row++;
                }

                // Create a Grid for each item to hold BoxView and StackLayout
                var itemGrid = new Grid
                {
                    RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
                    ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = GridLength.Auto }
            },
                    Padding = new Thickness(10), // Padding around each item grid
                    Margin = new Thickness(10)  // Margin around the grid cells
                };

                // Add rectangle (BoxView)
                var boxView = new BoxView
                {
                    Color = Colors.LightGray,
                    HeightRequest = 50, // Keep the height fixed
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand // Stretch across the entire row
                };


                // Add image
                var image = new Image
                {
                    Source = item.ImageUrl,
                    WidthRequest = 60,
                    HeightRequest = 60,
                    GestureRecognizers =
                    {
                    new TapGestureRecognizer
                        {
                    Command = new Command(() => ShowImagePopup(item.ImageUrl))
                        }
                    }
                };

                // Add labels and buttons
                var nameLabel = new Label
                {
                    Text = item.Name,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };

                var valueLabel = new Label
                {
                    Text = item.Value.ToString(),
                    FontSize = 18,  // Increase by 1.25x (default ~14)
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10, 0, 0, 0) // Space between - button and value
                };

                var plusButton = new Button
                {
                    Text = "+",
                    Command = new Command(() => AdjustValue(item, 1)),
                    BackgroundColor = Colors.Blue,
                    TextColor = Colors.White,
                    FontSize = 18, // Increase by 1.25x
                    WidthRequest = 40,  // Adjust if needed
                    HeightRequest = 40
                };

                var minusButton = new Button
                {
                    Text = "-",
                    Command = new Command(() => AdjustValue(item, -1)),
                    BackgroundColor = Colors.Blue,
                    TextColor = Colors.White,
                    FontSize = 18, // Increase by 1.25x
                    WidthRequest = 40,
                    HeightRequest = 40
                };


                // Create and add the layout for each item in the grid
                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { image, nameLabel, plusButton, minusButton, valueLabel }
                };

                // Add the BoxView and StackLayout to the itemGrid
                itemGrid.Children.Add(boxView); // Add BoxView at (0, 0) by default
                Grid.SetColumnSpan(boxView, 5); // Ensures the BoxView spans across all columns
                itemGrid.Children.Add(stackLayout); // Add StackLayout at (0, 1) by default

                // Set the Grid.Row and Grid.Column properties for itemGrid before adding it to the parent grid
                Grid.SetRow(itemGrid, row);
                Grid.SetColumn(itemGrid, col);

                // Add the itemGrid to the parent grid (ItemGridContent)
                ItemGridContent.Children.Add(itemGrid); // Now only passing the element to Add()

                col++; // Move to next column
            }
        }

        // Method to adjust the value when + or - button is clicked
        private void AdjustValue(Item item, int change)
        {
            // Prevent the value from going below 0
            if (item.Value + change >= 0)
            {
                item.Value += change;
            }
            PopulateGrid(Items); // Update grid with the new value
        }


        // Search functionality
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredItems = Items.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            PopulateGrid(filteredItems);
        }

        private void ShowImagePopup(string imageUrl)
        {
            var popupBackground = new BoxView
            {
                BackgroundColor = Colors.Black.MultiplyAlpha(0.8f), // Dim the background
                Opacity = 0.8,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var popupImage = new Image
            {
                Source = imageUrl,
                WidthRequest = 300, // Adjust size
                HeightRequest = 300,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var popupLayout = new AbsoluteLayout();

            AbsoluteLayout.SetLayoutBounds(popupBackground, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(popupBackground, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(popupImage, new Rect(0.5, 0.5, -1, -1));
            AbsoluteLayout.SetLayoutFlags(popupImage, AbsoluteLayoutFlags.PositionProportional);

            popupLayout.Children.Add(popupBackground);
            popupLayout.Children.Add(popupImage);

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) => this.Content = mainLayout; // Restore original layout
            popupBackground.GestureRecognizers.Add(tapGesture);
            popupImage.GestureRecognizers.Add(tapGesture);

            this.Content = popupLayout; // Show the popup
        }



        // Item class to hold data for each item
        public class Item
        {
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public int Value { get; set; }

            public Item(string name, string imageUrl, int value)
            {
                Name = name;
                ImageUrl = imageUrl;
                Value = value;
            }
        }
    }
}
